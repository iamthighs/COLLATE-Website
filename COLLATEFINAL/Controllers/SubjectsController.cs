 using COLLATEFINAL.Models;
using Microsoft.AspNetCore.Mvc;
using COLLATEFINAL.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using COLLATEFINAL.Data.Migrations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using COLLATEFINAL.Common;
using Microsoft.AspNetCore.Identity;
using System.Data;
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,Faculty")]
    public class SubjectsController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public SubjectsController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
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
        public IActionResult Create(SubjectModel subjectModel)
        {
            string uniqueFileName = UploadedFile(subjectModel);
            subjectModel.ImageUrl = uniqueFileName;

            string imgext = Path.GetExtension(subjectModel.CoverImage.FileName);
            if (imgext == ".jpg" || imgext == ".png")

            {


                _context.Add(subjectModel);
                _context.SaveChanges();
                TempData["success"] = "Subject created successfully.";
                return RedirectToAction(nameof(List));

            }
            else
            {
                ModelState.AddModelError("", "Uploaded file is not a jpg or png file!");
                TempData["error"] = "Uploaded file is not a jpg or png file!";
            }
            return View();

        }

        private string UploadedFile(SubjectModel subjectModel)
        {
            string uniqueFileName = subjectModel.ImageUrl;

            if (subjectModel.CoverImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads/Subjects");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + subjectModel.CoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    subjectModel.CoverImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        
        [HttpGet]
        // GET: SubjectModels/Edit/5
        public IActionResult Edit(int id)
        {

            // Retrieve data from the database
            var itemsFromDatabase = _context.Subjects.ToList();

            // Create a list of SelectListItem
            var category = itemsFromDatabase.Select(item => new SelectListItem
            {
                Value = item.Subject, 
                Text = item.Subject 
            }).ToList();

            ViewBag.category = category;

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

        // POST: SubjectModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SubjectModel subjectModel)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                string uniqueImg = UploadedFile(subjectModel);
                subjectModel.ImageUrl = uniqueImg;


                string imgext = Path.GetExtension(uniqueImg);
                if (imgext == ".jpg" || imgext == ".png")

                {


                    _context.Update(subjectModel);
                    _context.SaveChanges();
                    TempData["success"] = "Subject updated successfully";

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
            TempData["error"] = "Error when updating Subject";
            return View();
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
    }
}
