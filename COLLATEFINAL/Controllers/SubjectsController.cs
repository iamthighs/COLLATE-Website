using COLLATEFINAL.Common;
using COLLATEFINAL.Data;
using COLLATEFINAL.Data.Migrations;
using COLLATEFINAL.Helpers;
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
using System.Data;
using System.IO;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,Faculty")]
    public class SubjectsController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly BulkRepository _bulkRepository;
        private readonly SampleImportService _sampleImportService;
        private readonly FileHelper _file;


        public SubjectsController(ApplicationDbContext context, 
            IWebHostEnvironment webHost, 
            BulkRepository bulkRepository, 
            SampleImportService sampleImportService,
            FileHelper file)
        {
            _context = context;
            webHostEnvironment = webHost;
            _bulkRepository = bulkRepository;
            _sampleImportService = sampleImportService;
            _file = file;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {

            List<SubjectModel> subjectModels = _context.Subjects.ToList();

            return View(subjectModels);

        }

        public async Task<IActionResult> List(PaginatedRequest request)
        {


            var subjectModels = await _context.SubjectsGetPaginated(request.PageNumber, PaginatedRequest.ITEMS_PER_PAGE, request.SearchKeyword ?? string.Empty);

            subjectModels.SearchKeyword = request.SearchKeyword;
            return View(subjectModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            SubjectModel subjectModel = new SubjectModel();
            return View(subjectModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.CoverImage == null || model.CoverImage.Length == 0)
            {
                ModelState.AddModelError(nameof(model.CoverImage), "Cover image is required.");
                return View(model);
            }

            var allowedExtensions = new[] { ".jpg", ".png" };
            var imgExt = Path.GetExtension(model.CoverImage.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(imgExt))
            {
                ModelState.AddModelError(nameof(model.CoverImage), "Uploaded file must be JPG or PNG.");
                return View(model);
            }

            model.ImageUrl = await _file.SaveFileAsync(model.CoverImage, "Uploads/Subjects");

            await _context.Subjects.AddAsync(model);
            await _context.SaveChangesAsync();

            TempData["success"] = "Subject created successfully.";
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (_context.Subjects == null)
                return NotFound();

            var subject = _context.Subjects.Find(id);
            if (subject == null)
                return NotFound();

            ViewBag.category = _context.Subjects
                .Select(s => new SelectListItem
                {
                    Value = s.Subject,
                    Text = s.Subject
                })
                .ToList();

            var model = new EditSubjViewModel
            {
                Id = subject.Id,
                Subject = subject.Subject,

                Lectures = _context.Lectures
                    .Where(l => l.Subject == subject.Subject)
                    .ToList(),

                Videos = _context.Videos
                    .Where(v => v.Subject == subject.Subject)
                    .ToList()
            };

            return View(model);
        }


        [AllowAnonymous]
        [HttpGet]
        // GET: SubjectModels/Edit/5
        public IActionResult SubjectsDetail(int id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = _context.Subjects.Find(id);

            if (subject == null)
            {
                return NotFound();
            }

            var model = new EditSubjViewModel
            {
                Id = subject.Id,
                Subject = subject.Subject
            };


            foreach (var lecture in _context.Lectures)
            {
                if (subject.Subject == lecture.Subject)
                {
                    model.IsSelected = true;

                    if (model.IsSelected == true)
                    {
                        model.Lectures.Add(lecture);
                    }
                }
                else
                {
                    model.IsSelected = false;
                }






            }

            foreach (var videos in _context.Videos)
            {

                if (subject.Subject == videos.Subject)
                {
                    model.IsSelected = true;

                    if (model.IsSelected == true)
                    {
                        model.Videos.Add(videos);
                    }
                }
                else
                {
                    model.IsSelected = false;
                }




            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditSubjViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid form data.";
                return View(model); // ✅ correct type
            }

            try
            {
                var entity = _context.Subjects.Find(id);
                if (entity == null)
                    return NotFound();

                entity.Subject = model.Subject;


                _context.SaveChanges();
                TempData["success"] = "Subject updated successfully";
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Unexpected error occurred.";
                return View(model);
            }
        }



        [HttpGet]
        public async Task<IActionResult> EditLecsInSubj(int subjId)
        {
            ViewBag.subjId = subjId;

            var subject = _context.Subjects.Find(subjId);

            if (subject == null)
            {
                ViewBag.ErrorMessage = $"Subject with Id = {subjId} cannot be found";
                return View("NotFound");
            }

            var model = new List<LecSubjViewModel>();

            foreach (var lecture in _context.Lectures)
            {
                var lecSubjViewModel = new LecSubjViewModel
                {
                    LecId = lecture.Id,
                    Title = lecture.Title,
                    Subject = lecture.Subject
                };

                if (subject.Subject == lecture.Subject)
                {
                    lecSubjViewModel.IsSelected = true;
                }
                else
                {
                    lecSubjViewModel.IsSelected = false;
                }

                model.Add(lecSubjViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLecsInSubj(List<LecSubjViewModel> model, int subjId)
        {
            var subject = _context.Subjects.Find(subjId);

            if (subject == null)
            {
                ViewBag.ErrorMessage = $"Subject with Id = {subjId} cannot be found";
                return View("NotFound");
            }

            

            for (int i = 0; i < model.Count; i++)
            {
                var lecture = _context.Lectures.Find(model[i].LecId);


                var lecAndVid = new EditSubjViewModel
                {
                    Id = subject.Id,
                    Subject = subject.Subject
                };

                if (model[i].IsSelected)
                {
                    lecAndVid.Lectures.Add(lecture);

                }
                else if (!model[i].IsSelected)
                {
                    lecAndVid.Lectures.Remove(lecture);
                }
                else
                {
                    continue;
                }

                
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Edit", new { Id = subjId });
                
            }

            return RedirectToAction("Edit", new { Id = subjId });
        }


        // GET: SubjectModels/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = _context.Subjects
                .FirstOrDefault(m => m.Id == id);

            var subjectModel = new SubjectModel();
            if (subject == null)
            {
                return NotFound();
            }

            return View(subjectModel);
        }

        // POST: SubjectModels/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Subjects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subjects'  is null.");
            }
            var subjectModel = _context.Subjects.Find(id);
            if (subjectModel != null)
            {
                _context.Subjects.Remove(subjectModel);
            }
            
            string deleteImgFromFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads/Subjects");
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteImgFromFolder, subjectModel.ImageUrl);

            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            _context.SaveChanges();
            TempData["success"] = "Subject deleted successfully";
            return RedirectToAction(nameof(List));
        }

        private bool SubjectModelExists(int id)
        {
            return (_context.Subjects?.Any(e => e.Id == id)).GetValueOrDefault();
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
                var samples = _sampleImportService.ParseCsvFile<SubjectModel, SubjectCsvMap>(file);

                // Insert the samples into the database.
                _bulkRepository.BulkInsertEntities(samples);

                TempData["success"] = "Bulk import of subjects successful.";
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred during the bulk import: " + ex.Message;
            }

            return RedirectToAction("List");
        }
    }
}
