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
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,Faculty", Policy = "InstructionalPolicy")]
    public class PrototypeController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        
        public PrototypeController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {

            List<PrototypeModel> prototypeModels = _context.Prototypes.ToList();

            return View(prototypeModels);

        }

        [AllowAnonymous]
        public IActionResult IndexLecture()
        {

            List<LectureModel> lectureModels = _context.Lectures.ToList();

            return View(lectureModels);

        }

        [AllowAnonymous]
        public IActionResult IndexVideos()
        {

            List<VideosModel> videosModels = _context.Videos.ToList();

            return View(videosModels);

        }

        public async Task<IActionResult> List(PaginatedRequest request)
        {


            var prototypeModels = await _context.GetPaginated(request.PageNumber, PaginatedRequest.ITEMS_PER_PAGE, request.SearchKeyword ?? string.Empty);

            prototypeModels.SearchKeyword = request.SearchKeyword;
            return View(prototypeModels);
        }

        public async Task<IActionResult> ListLectures(PaginatedRequest request)
        {


            var lectureModels = await _context.LecturesGetPaginated(request.PageNumber, PaginatedRequest.ITEMS_PER_PAGE, request.SearchKeyword ?? string.Empty);

            lectureModels.SearchKeyword = request.SearchKeyword;
            return View(lectureModels);
        }

        public async Task<IActionResult> ListVideos(PaginatedRequest request)
        {


            var videosModels = await _context.VideosGetPaginated(request.PageNumber, PaginatedRequest.ITEMS_PER_PAGE, request.SearchKeyword ?? string.Empty);

            videosModels.SearchKeyword = request.SearchKeyword;
            return View(videosModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PrototypeModel prototypeModel = new PrototypeModel();
            return View(prototypeModel);
        }
        
        [HttpGet]
        public IActionResult CreateLecture()
        {
            var itemsFromDatabase = _context.Subjects.ToList();

            // Create a list of SelectListItem
            var category = itemsFromDatabase.Select(item => new SelectListItem
            {
                Value = item.Subject,
                Text = item.Subject
            }).ToList();

            category.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "Select an item"
            });

            ViewBag.category = category;
            LectureModel lectureModel = new LectureModel();
            
            return View(lectureModel);
        }

        [HttpGet]
        public IActionResult CreateVideos()
        {
            var itemsFromDatabase = _context.Subjects.ToList();

            // Create a list of SelectListItem
            var category = itemsFromDatabase.Select(item => new SelectListItem
            {
                Value = item.Subject,
                Text = item.Subject
            }).ToList();

            category.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "Select an item"
            });

            ViewBag.category = category;
            VideosModel videosModel = new VideosModel();
            return View(videosModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PrototypeModel prototypeModel)
        {
            string uniqueFileName = UploadedFile(prototypeModel);
            string uniqueFilePDF = UploadedPDF(prototypeModel);
            prototypeModel.ImageUrl = uniqueFileName;
            prototypeModel.FileUrl = uniqueFilePDF;
            _context.Add(prototypeModel);
            _context.SaveChanges();
            TempData["success"] = "Instructional Material created successfully.";
            return RedirectToAction(nameof(List));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLecture(LectureModel lectureModel)
        {
            string uniqueFilePDF = UploadedLecturePDF(lectureModel);
            lectureModel.FileUrl = uniqueFilePDF;

            string imgext = Path.GetExtension(lectureModel.UploadedPDFFile.FileName);
            if (imgext == ".pdf")

            {


                _context.Add(lectureModel);
                _context.SaveChanges();
                TempData["success"] = "Lecture created successfully.";
                return RedirectToAction(nameof(ListLectures));

            }
            else
            {
                ModelState.AddModelError("", "Uploaded file is not a pdf file!");
                TempData["error"] = "Uploaded file is not a pdf file!";
            }
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateVideos(VideosModel videosModel)
        {
            
            _context.Add(videosModel);
            _context.SaveChanges();
            TempData["success"] = "Videos created successfully.";
            return RedirectToAction(nameof(ListVideos));

        }

        private string UploadedFile(PrototypeModel prototypeModel)
        {
            string uniqueFileName = prototypeModel.ImageUrl;

            if (prototypeModel.CoverImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + prototypeModel.CoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    prototypeModel.CoverImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private string UploadedPDF(PrototypeModel prototypeModel)
        {
            string uniqueFileName = prototypeModel.FileUrl;

            if (prototypeModel.UploadedCoverImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "PDF/Lectures");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + prototypeModel.UploadedCoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    prototypeModel.UploadedCoverImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string UploadedLecturePDF(LectureModel lectureModel)
        {
            string uniqueFileName = lectureModel.FileUrl;

            if (lectureModel.UploadedPDFFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "PDF/Lectures");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + lectureModel.UploadedPDFFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    lectureModel.UploadedPDFFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        [HttpGet]
        // GET: PrototypeModels/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null || _context.Prototypes == null)
            {
                return NotFound();
            }

            var prototype = _context.Prototypes.Find(id);

            if (prototype == null)
            {
                return NotFound();
            }
            return View(prototype);
        }

        [HttpGet]
        // GET: LectureModels/Edit/5
        public IActionResult EditLecture(int id)
        {
            var itemsFromDatabase = _context.Subjects.ToList();

            // Create a list of SelectListItem
            var category = itemsFromDatabase.Select(item => new SelectListItem
            {
                Value = item.Subject,
                Text = item.Subject
            }).ToList();

            category.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "Select an item"
            });

            ViewBag.category = category;
            if (id == null || _context.Lectures == null)
            {
                return NotFound();
            }

            var lecture = _context.Lectures.Find(id);

            if (lecture == null)
            {
                return NotFound();
            }
            return View(lecture);
        }

        [HttpGet]
        // GET: VideosModels/Edit/5
        public IActionResult EditVideos(int id)
        {
            var itemsFromDatabase = _context.Subjects.ToList();

            // Create a list of SelectListItem
            var category = itemsFromDatabase.Select(item => new SelectListItem
            {
                Value = item.Subject,
                Text = item.Subject
            }).ToList();

            category.Insert(0, new SelectListItem
            {
                Value = string.Empty,
                Text = "Select an item"
            });

            ViewBag.category = category;
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var videos = _context.Videos.Find(id);

            if (videos == null)
            {
                return NotFound();
            }
            return View(videos);
        }

        // POST: PrototypeModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PrototypeModel prototypeModel)
        {
            if (id == null || _context.Prototypes == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                string uniqueImg = UploadedFile(prototypeModel);
                string uniqueFile = UploadedPDF(prototypeModel);
                prototypeModel.ImageUrl = uniqueImg;
                prototypeModel.FileUrl = uniqueFile;
                _context.Update(prototypeModel);
                _context.SaveChanges();
                TempData["success"] = "Instructional Material updated successfully";

                return RedirectToAction(nameof(List));
            }
            ModelState.AddModelError("name", "");
            TempData["error"] = "Error when updating Instructional Material";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditLecture(int id, LectureModel lectureModel)
        {
            if (id == null || _context.Lectures == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string uniqueFile = UploadedLecturePDF(lectureModel);
                lectureModel.FileUrl = uniqueFile;


                string imgext = Path.GetExtension(uniqueFile);
                if (imgext == ".pdf")

                {


                    _context.Update(lectureModel);
                    _context.SaveChanges();
                    TempData["success"] = "Lecture updated successfully";

                    return RedirectToAction(nameof(ListLectures));

                }
                else
                {
                    ModelState.AddModelError("", "Uploaded file is not a jpg or png file!");
                    TempData["error"] = "Uploaded file is not a jpg or png file!";
                }


                return RedirectToAction(nameof(Edit));
            }
            ModelState.AddModelError("name", "");
            TempData["error"] = "Error when updating Lecture";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditVideos(int id, VideosModel videosModel)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                
                _context.Update(videosModel);
                _context.SaveChanges();
                TempData["success"] = "Videos updated successfully";

                return RedirectToAction(nameof(ListVideos));
            }
            ModelState.AddModelError("name", "");
            TempData["error"] = "Error when updating Video";
            return View();
        }


        // GET: PrototypeModels/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null || _context.Prototypes == null)
            {
                return NotFound();
            }

            var prototype = _context.Prototypes
                .FirstOrDefault(m => m.Id == id);

            var prototypeModel = new PrototypeModel();
            if (prototype == null)
            {
                return NotFound();
            }

            return View(prototypeModel);
        }

        // POST: PrototypeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Prototypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Prototypes'  is null.");
            }
            var prototypeModel = _context.Prototypes.Find(id);
            if (prototypeModel != null)
            {
                _context.Prototypes.Remove(prototypeModel);
            }
            string deleteFileFromFolder = Path.Combine(webHostEnvironment.WebRootPath, "PDF/Lectures");
            var CurrentFile = Path.Combine(Directory.GetCurrentDirectory(), deleteFileFromFolder, prototypeModel.FileUrl);

            if (System.IO.File.Exists(CurrentFile))
            {
                System.IO.File.Delete(CurrentFile);
            }
            string deleteImgFromFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads/Subjects");
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteImgFromFolder, prototypeModel.ImageUrl);

            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            _context.SaveChanges();
            TempData["success"] = "Instructional Material deleted successfully";
            return RedirectToAction(nameof(List));
        }

        [HttpPost, ActionName("DeleteConfirmedLecture")]
        public IActionResult DeleteConfirmedLecture(int id)
        {
            if (_context.Lectures == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Lectures'  is null.");
            }
            var lectureModel = _context.Lectures.Find(id);
            if (lectureModel != null)
            {
                _context.Lectures.Remove(lectureModel);
            }
            string deleteFileFromFolder = Path.Combine(webHostEnvironment.WebRootPath, "PDF");
            var CurrentFile = Path.Combine(Directory.GetCurrentDirectory(), deleteFileFromFolder, lectureModel.FileUrl);

            if (System.IO.File.Exists(CurrentFile))
            {
                System.IO.File.Delete(CurrentFile);
            }
            
            _context.SaveChanges();
            TempData["success"] = "Lecture deleted successfully";
            return RedirectToAction(nameof(ListLectures));
        }

        [HttpPost, ActionName("DeleteConfirmedVideos")]
        public IActionResult DeleteConfirmedVideos(int id)
        {
            if (_context.Videos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Videos'  is null.");
            }
            var videosModel = _context.Videos.Find(id);
            if (videosModel != null)
            {
                _context.Videos.Remove(videosModel);
            }
            

            _context.SaveChanges();
            TempData["success"] = "Video deleted successfully";
            return RedirectToAction(nameof(ListLectures));
        }

        private bool PrototypeModelExists(int id)
        {
            return (_context.Prototypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool LectureModelExists(int id)
        {
            return (_context.Lectures?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool VideosModelExists(int id)
        {
            return (_context.Videos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
