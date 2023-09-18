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
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,sceneOfficer", Policy = "ResearchPolicy")]
    public class ResearchPapersController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;



        public ResearchPapersController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
            

        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(PaginatedRequest request)
        {
            

            var researchPapersModels = await _context.ResearchGetPaginated(request.PageNumber, PaginatedRequest.ITEMS_PER_PAGE, request.SearchKeyword ?? string.Empty);
            researchPapersModels.SearchKeyword = request.SearchKeyword;
            return View(researchPapersModels);
        }

        public IActionResult List()
        {

            List<ResearchPapersModel> researchPapersModels = _context.ResearchPapers.ToList();

            return View(researchPapersModels);

        }
        

        [HttpGet]
        public IActionResult Create()
        {
            //Creating the List of SelectListItem, this list you can bind from the database.
            List<SelectListItem> category = new()
            {
                new SelectListItem { Value = "Thesis", Text = "Thesis" },
                new SelectListItem { Value = "Case Study", Text = "Case Study" },
                new SelectListItem { Value = "Hardware", Text = "Hardware" },
                new SelectListItem { Value = "Software", Text = "Software" },
                new SelectListItem { Value = "Others", Text = "Others" }
            };

            //assigning SelectListItem to view Bag
            ViewBag.category = category;

            List<SelectListItem> yearandsec = new()
            {
                new SelectListItem { Value = "BSCPE 1-1", Text = "BSCPE 1-1" },
                new SelectListItem { Value = "BSCPE 2-1", Text = "BSCPE 2-1" },
                new SelectListItem { Value = "BSCPE 3-1", Text = "BSCPE 3-1" },
                new SelectListItem { Value = "BSCPE 4-1", Text = "BSCPE 4-1" }
            };

            //assigning SelectListItem to view Bag
            ViewBag.yearandsec = yearandsec;
            ResearchPapersModel researchPaperModel = new ResearchPapersModel();
            return View(researchPaperModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ResearchPapersModel researchPaperModel)
        {
            string uniqueFileName = UploadedFile(researchPaperModel);
            string uniqueFilePDF = UploadedPDF(researchPaperModel);
            researchPaperModel.ImageUrl = uniqueFileName;
            researchPaperModel.FileUrl = uniqueFilePDF;

            string imgext = Path.GetExtension(researchPaperModel.CoverImage.FileName);
            string fileext = Path.GetExtension(researchPaperModel.UploadedCoverImage.FileName);
            if (imgext == ".jpg" || imgext == ".png" && fileext == ".pdf")

            {

                _context.Add(researchPaperModel);
                _context.SaveChanges();
                TempData["success"] = "Research Paper created successfully.";
                return RedirectToAction(nameof(List));

            }
            else
            {
                ModelState.AddModelError("", "Uploaded file is not recognize. Please upload the correct file type");
                TempData["error"] = "Uploaded file is not recognize. Please upload the correct file type";
            }
            return View();

        }

        private string UploadedFile(ResearchPapersModel researchPaperModel)
        {
            string uniqueFileName = researchPaperModel.ImageUrl;

            if (researchPaperModel.CoverImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads/ResearchPapers");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + researchPaperModel.CoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    researchPaperModel.CoverImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private string UploadedPDF(ResearchPapersModel researchPaperModel)
        {
            string uniqueFileName = researchPaperModel.FileUrl;

            if (researchPaperModel.UploadedCoverImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "PDF/ResearchPapers");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + researchPaperModel.UploadedCoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    researchPaperModel.UploadedCoverImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //Creating the List of SelectListItem, this list you can bind from the database.
            List<SelectListItem> category = new()
            {
                new SelectListItem { Value = "Thesis", Text = "Thesis" },
                new SelectListItem { Value = "Case Study", Text = "Case Study" },
                new SelectListItem { Value = "Hardware", Text = "Hardware" },
                new SelectListItem { Value = "Software", Text = "Software" },
                new SelectListItem { Value = "Others", Text = "Others" }
            };

            //assigning SelectListItem to view Bag
            ViewBag.category = category;

            List<SelectListItem> yearandsec = new()
            {
                new SelectListItem { Value = "BSCPE 1-1", Text = "BSCPE 1-1" },
                new SelectListItem { Value = "BSCPE 2-1", Text = "BSCPE 2-1" },
                new SelectListItem { Value = "BSCPE 3-1", Text = "BSCPE 3-1" },
                new SelectListItem { Value = "BSCPE 4-1", Text = "BSCPE 4-1" }
            };

            //assigning SelectListItem to view Bag
            ViewBag.yearandsec = yearandsec;
            if (id == null || _context.ResearchPapers == null)
            {
                return NotFound();
            }

            var researchPaper = _context.ResearchPapers.Find(id);

            if (researchPaper == null)
            {
                return NotFound();
            }
            return View(researchPaper);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ResearchPapersModel researchPaperModel)
        {
            if (id == null || _context.ResearchPapers == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                string uniqueImg = UploadedFile(researchPaperModel);
                string uniqueFile = UploadedPDF(researchPaperModel);
                researchPaperModel.ImageUrl = uniqueImg;
                researchPaperModel.FileUrl = uniqueFile;

                string imgext = Path.GetExtension(uniqueImg);
                string fileext = Path.GetExtension(uniqueFile);
                if (imgext == ".jpg" || imgext == ".png" && fileext == ".pdf")

                {

                    _context.Update(researchPaperModel);
                    _context.SaveChanges();
                    TempData["success"] = "Research Paper updated successfully";

                    return RedirectToAction(nameof(List));

                }
                else
                {
                    ModelState.AddModelError("", "Uploaded file is not recognize. Please upload the correct file type");
                    TempData["error"] = "Uploaded file is not recognize. Please upload the correct file type";
                }
                return RedirectToAction(nameof(Edit));
            }
            ModelState.AddModelError("name", "");
            TempData["error"] = "Error when updating Research Paper";
            return View();
        }



        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null || _context.ResearchPapers == null)
            {
                return NotFound();
            }

            var researchPaper = _context.ResearchPapers
                .FirstOrDefault(m => m.Id == id);

            var researchPaperModel = new ResearchPapersModel();
            if (researchPaper == null)
            {
                return NotFound();
            }

            return View(researchPaperModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.ResearchPapers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ResearchPapers'  is null.");
            }
            var researchPaperModel = _context.ResearchPapers.Find(id);
            if (researchPaperModel != null)
            {
                _context.ResearchPapers.Remove(researchPaperModel);
            }
            string deleteFileFromFolder = Path.Combine(webHostEnvironment.WebRootPath, "PDF/ResearchPapers");
            var CurrentFile = Path.Combine(Directory.GetCurrentDirectory(), deleteFileFromFolder, researchPaperModel.FileUrl);

            if (System.IO.File.Exists(CurrentFile))
            {
                System.IO.File.Delete(CurrentFile);
            }
            string deleteImgFromFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteImgFromFolder, researchPaperModel.ImageUrl);

            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            _context.SaveChanges();
            TempData["success"] = "Research paper deleted successfully";
            return RedirectToAction(nameof(List));
        }

        private bool ResearchPaperModelExists(int id)
        {
            return (_context.ResearchPapers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
