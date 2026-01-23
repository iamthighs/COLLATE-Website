using COLLATE.Helpers.Common;
using COLLATE.Helpers.Data;
using COLLATEFINAL.Data.Migrations;
using COLLATE.Helpers.Helpers;
using COLLATE.Helpers.Models;
using COLLATEFINAL.Repository;
using COLLATEFINAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,sceneOfficer")]
    public class ResearchPapersController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly BulkRepository _bulkRepository;
        private readonly SampleImportService _sampleImportService;
        private readonly FileHelper _file;



        public ResearchPapersController(ApplicationDbContext context, 
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
        public async Task<IActionResult> Create(ResearchPapersModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.CoverImage == null || model.CoverImage.Length == 0)
            {
                ModelState.AddModelError(nameof(model.CoverImage), "Cover image is required.");
                return View(model);
            }

            var imageExt = Path.GetExtension(model.CoverImage.FileName).ToLowerInvariant();
            var allowedImageExt = new[] { ".jpg", ".png" };

            if (!allowedImageExt.Contains(imageExt))
            {
                ModelState.AddModelError(nameof(model.CoverImage), "Image must be JPG or PNG.");
                return View(model);
            }

            if (model.UploadedCoverImage == null || model.UploadedCoverImage.Length == 0)
            {
                ModelState.AddModelError(nameof(model.UploadedCoverImage), "PDF file is required.");
                return View(model);
            }

            var pdfExt = Path.GetExtension(model.UploadedCoverImage.FileName).ToLowerInvariant();
            if (pdfExt != ".pdf")
            {
                ModelState.AddModelError(nameof(model.UploadedCoverImage), "File must be a PDF.");
                return View(model);
            }

            model.ImageUrl = await _file.SaveFileAsync(model.CoverImage, "Uploads/ResearchPapers");
            model.FileUrl = await _file.SaveFileAsync(model.UploadedCoverImage, "PDF/ResearchPapers");

            await _context.ResearchPapers.AddAsync(model);
            await _context.SaveChangesAsync();

            TempData["success"] = "Research Paper created successfully.";
            return RedirectToAction(nameof(List));
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
        public async Task<IActionResult> Edit(int id, ResearchPapersModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var existing = await _context.ResearchPapers.FindAsync(id);
            if (existing == null)
                return NotFound();
            existing.Header = model.Header;
            existing.Title = model.Title;
            existing.YearSec = model.YearSec;
            existing.Description = model.Description;
            existing.Authors = model.Authors;
            existing.PostedDate = model.PostedDate;

            if (model.CoverImage != null && model.CoverImage.Length > 0)
            {
                var imageExt = Path.GetExtension(model.CoverImage.FileName).ToLowerInvariant();
                var allowedImageExt = new[] { ".jpg", ".png" };

                if (!allowedImageExt.Contains(imageExt))
                {
                    ModelState.AddModelError(nameof(model.CoverImage), "Image must be JPG or PNG.");
                    return View(model);
                }

                existing.ImageUrl = await _file.SaveFileAsync(model.CoverImage, "Uploads/ResearchPapers");
            }

            if (model.UploadedCoverImage != null && model.UploadedCoverImage.Length > 0)
            {
                var pdfExt = Path.GetExtension(model.UploadedCoverImage.FileName).ToLowerInvariant();

                if (pdfExt != ".pdf")
                {
                    ModelState.AddModelError(nameof(model.UploadedCoverImage), "File must be a PDF.");
                    return View(model);
                }

                existing.FileUrl = await _file.SaveFileAsync(model.UploadedCoverImage, "PDF/ResearchPapers");
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Research Paper updated successfully.";
            return RedirectToAction(nameof(List));
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
                var samples = _sampleImportService.ParseCsvFile<ResearchPapersModel, ResearchPaperCsvMap>(file);

                // Insert the samples into the database.
                _bulkRepository.BulkInsertEntities(samples);

                TempData["success"] = "Bulk import of research papers successful.";
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred during the bulk import: " + ex.Message;
            }

            return RedirectToAction("List");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMultiple(List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest();

            var researchPapers = _context.ResearchPapers.Where(x => ids.Contains(x.Id));

            _context.ResearchPapers.RemoveRange(researchPapers);
            _context.SaveChanges();

            return Ok();
        }
    }
}
