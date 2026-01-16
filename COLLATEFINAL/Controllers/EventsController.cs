using COLLATEFINAL.Common;
using COLLATEFINAL.Data;
using COLLATEFINAL.Data.Migrations;
using COLLATEFINAL.Models;
using COLLATEFINAL.Repository;
using COLLATEFINAL.Services;
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,sceneOfficer")]
    public class EventsController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly BulkRepository _bulkRepository;
        private readonly SampleImportService _sampleImportService;

        public EventsController(ApplicationDbContext context, IWebHostEnvironment webHost, UserManager<AppIdentityUser> userManager, BulkRepository bulkRepository, SampleImportService sampleImportService)
        {
            _context = context;
            webHostEnvironment = webHost;
            this.userManager = userManager;
            _bulkRepository = bulkRepository;
            _sampleImportService = sampleImportService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {

            List<EventsModel> eventsModels = _context.Events.Include(e => e.Attendees).ToList();

            return View(eventsModels);

        }

        

        public IEnumerable<AppIdentityUser> GetAllUsers()
        {
            return userManager.Users.ToList();
        }
        
        public IActionResult Details(int id)
        {
            var @event = _context.Events.Include(e => e.Attendees).FirstOrDefault(e => e.Id == id);
            return View(@event);
        }

        [AllowAnonymous]
        public IActionResult RegisterForEvent(int id)
        {
            var userId = userManager.GetUserId(User);
            ViewBag.UserId = userId;

            var @event = _context.Events.Include(e => e.Attendees).FirstOrDefault(e => e.Id == id);
            return View(@event);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterForEventConfirm(int eventId)
        {
            var @event = _context.Events.Include(e => e.Attendees).FirstOrDefault(e => e.Id == eventId);

            var userId = userManager.GetUserId(User);
            ViewBag.UserId = userId;
            var user = await userManager.FindByIdAsync(userId);


            if (@event == null || user == null)
            {
                return NotFound();
            }

            if (!@event.Attendees.Contains(user))
            {
                @event.Attendees.Add(user);
                await _context.SaveChangesAsync();
                TempData["success"] = "Successfully Registered in this Event";
            }
            else
            {
                TempData["error"] = "You already Register for this Event";
                return RedirectToAction("RegisterForEvent", new { id = eventId });
            }

            return RedirectToAction("RegisterForEvent", new { id = eventId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromEvent(int eventId, string userId)
        {
            var @event = _context.Events.Include(e => e.Attendees).FirstOrDefault(e => e.Id == eventId);
            var user = await userManager.FindByIdAsync(userId);

            if (@event == null || user == null)
            {
                return NotFound();
            }

            if (@event.Attendees.Contains(user))
            {
                @event.Attendees.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                @event.Attendees.Add(user);
            }

            return RedirectToAction("Details", new { id = eventId });
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToEvent(int eventId, string userId)
        {
            var @event = _context.Events.Include(e => e.Attendees).FirstOrDefault(e => e.Id == eventId);
            var user = await userManager.FindByIdAsync(userId);
            

            if (@event == null || user == null)
            {
                return NotFound();
            }
             
            if (!@event.Attendees.Contains(user))
            {
                @event.Attendees.Add(user);
                await _context.SaveChangesAsync();
                TempData["success"] = "Successfully Registered in this Event";
            }
            else
            {
                TempData["error"] = "This User already registered for this Event";
                return RedirectToAction("Details", new { id = eventId });
            }
            TempData["error"] = "Error when Registering in an Event";
            return RedirectToAction("Details", new { id = eventId });
        }

        





        [Authorize(Roles = "Administrator,sceneOfficer")]
        public async Task<IActionResult> List(PaginatedRequest request)
        {


            var eventsModels = await _context.EventsGetPaginated(request.PageNumber, PaginatedRequest.ITEMS_PER_PAGE, request.SearchKeyword ?? string.Empty);

            eventsModels.SearchKeyword = request.SearchKeyword;
            return View(eventsModels);
        }


        [HttpGet]
        public IActionResult Create()
        {
            //Creating the List of SelectListItem, this list you can bind from the database.
            List<SelectListItem> category = new()
            {
                new SelectListItem { Value = "Webinar", Text = "Webinar" },
                new SelectListItem { Value = "Seminar", Text = "Seminar" },
                new SelectListItem { Value = "Workshop", Text = "Workshop" },
                new SelectListItem { Value = "Podcast", Text = "Podcast" },
                new SelectListItem { Value = "Sports", Text = "Sports" }
            };
            category.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "Select an item"
            });

            category.Add(new SelectListItem
            {
                Value = "Others",
                Text = "Others"
            });
            //assigning SelectListItem to view Bag
            ViewBag.category = category;

            EventsModel eventsModel = new EventsModel();
            return View(eventsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventsModel eventsModel)
        {
            


            string uniqueFileName = UploadedFile(eventsModel);
            eventsModel.ImageUrl = uniqueFileName;


            string imgext = Path.GetExtension(eventsModel.CoverImage.FileName);
            if (imgext == ".jpg" || imgext == ".png")

            {

                _context.Add(eventsModel);
                _context.SaveChanges();
                TempData["success"] = "Events created successfully.";

                return RedirectToAction(nameof(List));

            }
            else
            {
                ModelState.AddModelError("", "Uploaded file is not a jpg or png file!");
                TempData["error"] = "Uploaded file is not a jpg or png file!";
            }
            return View();

        }

        private string UploadedFile(EventsModel eventsModel)
        {
            string uniqueFileName = eventsModel.ImageUrl;

            if (eventsModel.CoverImage != null)
            {

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads/Events");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + eventsModel.CoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    eventsModel.CoverImage.CopyTo(fileStream);
                }
            }
            
            return uniqueFileName;
        }

        
        [HttpGet]
        // GET: EventsModels/Edit/5
        public IActionResult Edit(int id)
        {
            //Creating the List of SelectListItem, this list you can bind from the database.
            List<SelectListItem> category = new()
            {
                new SelectListItem { Value = "Webinar", Text = "Webinar" },
                new SelectListItem { Value = "Seminar", Text = "Seminar" },
                new SelectListItem { Value = "Workshop", Text = "Workshop" },
                new SelectListItem { Value = "Podcast", Text = "Podcast" },
                new SelectListItem { Value = "Sports", Text = "Sports" },
                new SelectListItem { Value = "Others", Text = "Others" }
            };

            //assigning SelectListItem to view Bag
            ViewBag.category = category;
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var events = _context.Events.Find(id);

            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // POST: EventsModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(int id, EventsModel eventsModel)
        {
            
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                string uniqueImg = UploadedFile(eventsModel);
                eventsModel.ImageUrl = uniqueImg;
                string imgext = Path.GetExtension(uniqueImg);
                if (imgext == ".jpg" || imgext == ".png")

                {

                    _context.Update(eventsModel);
                    _context.SaveChanges();
                    TempData["success"] = "Events updated successfully";

                    return RedirectToAction(nameof(List));

                }
                else
                {
                    ModelState.AddModelError("", "Uploaded file is not a jpg or png file!");
                    TempData["error"] = "Uploaded file is not a jpg or png file!";
                }
                

                return RedirectToAction(nameof(Edit));
            }
            ModelState.AddModelError("name", "");
            TempData["error"] = "Error when updating Events";
            return View();
        }



        // GET: EventsModels/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var events = _context.Events
                .FirstOrDefault(m => m.Id == id);

            var eventsModel = new EventsModel();
            if (events == null)
            {
                return NotFound();
            }

            return View(eventsModel);
        }

        // POST: EventsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Events'  is null.");
            }
            var eventsModel = _context.Events.Find(id);
            if (eventsModel != null)
            {
                _context.Events.Remove(eventsModel);
            }
            
            string deleteImgFromFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads/Events");
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteImgFromFolder, eventsModel.ImageUrl);

            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            _context.SaveChanges();
            TempData["success"] = "Events deleted successfully";
            return RedirectToAction(nameof(List));
        }

        private bool EventsModelExists(int id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public IActionResult BulkImportSamples(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                TempData["error"] = "Please select a valid file for import.";
                return RedirectToAction("List"); 
            }

            try
            {
                // Parse the uploaded file and create a collection of objects.
                var samples = _sampleImportService.ParseCsvFile<EventsModel, EventCsvMap>(file);

                // Insert the samples into the database.
                _bulkRepository.BulkInsertEntities(samples);

                TempData["success"] = "Bulk import of events successful.";
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred during the bulk import: " + ex.Message;
            }

            return RedirectToAction("List"); 
        }
    }
}
