using COLLATE.Helpers.Common;
using COLLATE.Helpers.Data;
using COLLATEFINAL.Data.Migrations;
using COLLATE.Helpers.Helpers;
using COLLATE.Helpers.Models;
using COLLATEFINAL.Repository;
using COLLATEFINAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,sceneOfficer")]
    public class GameAndWebDevController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly BulkRepository _bulkRepository;
        private readonly SampleImportService _sampleImportService;
        private readonly FileHelper _file;
        public GameAndWebDevController(ApplicationDbContext context, 
            IWebHostEnvironment webHost, BulkRepository bulkRepository, 
            SampleImportService sampleImportService,
            FileHelper file)
        {
            _context = context;
            webHostEnvironment = webHost;
            _bulkRepository = bulkRepository;
            _sampleImportService = sampleImportService;
            _file = file;
        }

        public IActionResult List()
        {

            List<GameAndWebDevModel> gameAndWebDevModels = _context.GameAndWebDevelopments.ToList();
            gameAndWebDevModels = gameAndWebDevModels.OrderBy(m => m.Title).ToList();
            return View(gameAndWebDevModels);

        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(PaginatedRequest request)
        {

            var gameAndWebModels = await _context.SoftwareGetPaginated(request.PageNumber, PaginatedRequest.ITEMS_PER_PAGE, request.SearchKeyword ?? string.Empty);

            gameAndWebModels.SearchKeyword = request.SearchKeyword;
            return View(gameAndWebModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> yearandsec = new()
            {
                new SelectListItem { Value = "BSCPE 1-1", Text = "BSCPE 1-1" },
                new SelectListItem { Value = "BSCPE 2-1", Text = "BSCPE 2-1" },
                new SelectListItem { Value = "BSCPE 3-1", Text = "BSCPE 3-1" },
                new SelectListItem { Value = "BSCPE 4-1", Text = "BSCPE 4-1" }
            };

            //assigning SelectListItem to view Bag
            ViewBag.yearandsec = yearandsec;
            GameAndWebDevModel gameAndWebDevModel = new GameAndWebDevModel();
            return View(gameAndWebDevModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameAndWebDevModel model)
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

            model.ImageUrl = await _file.SaveFileAsync(model.CoverImage, "Uploads/SoftwareProjects");

            await _context.GameAndWebDevelopments.AddAsync(model);
            await _context.SaveChangesAsync();

            TempData["success"] = "Software Project created successfully.";
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            List<SelectListItem> yearandsec = new()
            {
                new SelectListItem { Value = "BSCPE 1-1", Text = "BSCPE 1-1" },
                new SelectListItem { Value = "BSCPE 2-1", Text = "BSCPE 2-1" },
                new SelectListItem { Value = "BSCPE 3-1", Text = "BSCPE 3-1" },
                new SelectListItem { Value = "BSCPE 4-1", Text = "BSCPE 4-1" }
            };

            //assigning SelectListItem to view Bag
            ViewBag.yearandsec = yearandsec;
            if (id == null || _context.GameAndWebDevelopments == null)
            {
                return NotFound();
            }

            var gameAndWebDev = _context.GameAndWebDevelopments.Find(id);

            if (gameAndWebDev == null)
            {
                return NotFound();
            }
            return View(gameAndWebDev);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameAndWebDevModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var existing = await _context.GameAndWebDevelopments.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Update scalar fields only (avoid overposting)
            existing.GroupName = model.GroupName;
            existing.Title = model.Title;
            existing.YearSec = model.YearSec;
            existing.PostedDate = model.PostedDate;
            existing.VidLink = model.VidLink;
            existing.Description = model.Description;
            existing.DevelopersName = model.DevelopersName;
            existing.GameLink= model.GameLink;
            // add other properties as needed

            // Optional image update
            if (model.CoverImage != null && model.CoverImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".png" };
                var imgExt = Path.GetExtension(model.CoverImage.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(imgExt))
                {
                    ModelState.AddModelError(nameof(model.CoverImage), "Uploaded file must be JPG or PNG.");
                    return View(model);
                }

                existing.ImageUrl = await _file.SaveFileAsync(model.CoverImage, "Uploads/SoftwareProjects");
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Software Project updated successfully.";
            return RedirectToAction(nameof(List));
        }




        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null || _context.GameAndWebDevelopments == null)
            {
                return NotFound();
            }

            var gameAndWebDev = _context.GameAndWebDevelopments
                .FirstOrDefault(m => m.Id == id);

            var gameAndWebDevModel = new GameAndWebDevModel();
            if (gameAndWebDev == null)
            {
                return NotFound();
            }

            return View(gameAndWebDevModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.GameAndWebDevelopments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GameAndWebDevelopments'  is null.");
            }
            var gameAndWebDevModel = _context.GameAndWebDevelopments.Find(id);
            if (gameAndWebDevModel != null)
            {
                _context.GameAndWebDevelopments.Remove(gameAndWebDevModel);
            }
            string deleteImgFromFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteImgFromFolder, gameAndWebDevModel.ImageUrl);

            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            _context.SaveChanges();
            TempData["success"] = "Software Project deleted successfully";
            return RedirectToAction(nameof(List));
        }

        private bool GameAndWebDevModelExists(int id)
        {
            return (_context.GameAndWebDevelopments?.Any(e => e.Id == id)).GetValueOrDefault();
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
                var samples = _sampleImportService.ParseCsvFile<GameAndWebDevModel, SoftwareProjectCsvMap>(file);

                // Insert the samples into the database.
                _bulkRepository.BulkInsertEntities(samples);

                TempData["success"] = "Bulk import of software projects successful.";
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred during the bulk import: " + ex.Message;
            }

            return RedirectToAction("List");
        }

    }
}
