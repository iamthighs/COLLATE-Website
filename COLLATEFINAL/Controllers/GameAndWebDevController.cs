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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,sceneOfficer")]
    public class GameAndWebDevController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public GameAndWebDevController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
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
        public IActionResult Create(GameAndWebDevModel gameAndWebDevModel)
        {
            string uniqueFileName = UploadedFile(gameAndWebDevModel);
            gameAndWebDevModel.ImageUrl = uniqueFileName;

            string imgext = Path.GetExtension(gameAndWebDevModel.CoverImage.FileName);
            if (imgext == ".jpg" || imgext == ".png")

            {

                _context.Add(gameAndWebDevModel);
                _context.SaveChanges();
                TempData["success"] = "Software Project created successfully.";
                return RedirectToAction(nameof(List));

            }
            else
            {
                ModelState.AddModelError("", "Uploaded file is not a jpg or png file!");
                TempData["error"] = "Uploaded file is not a jpg or png file!";
            }

            return View();
        }

        private string UploadedFile(GameAndWebDevModel gameAndWebDevModel)
        {
            string uniqueFileName = gameAndWebDevModel.ImageUrl;

            if (gameAndWebDevModel.CoverImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads/SoftwareProjects");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + gameAndWebDevModel.CoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    gameAndWebDevModel.CoverImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
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
        public IActionResult Edit(int id, GameAndWebDevModel gameAndWebDevModel)
        {
            if (id == null || _context.GameAndWebDevelopments == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string uniqueImg = UploadedFile(gameAndWebDevModel);
                gameAndWebDevModel.ImageUrl = uniqueImg;

                string imgext = Path.GetExtension(uniqueImg);
                if (imgext == ".jpg" || imgext == ".png")

                {

                    _context.Update(gameAndWebDevModel);
                    _context.SaveChanges();
                    TempData["success"] = "Software Project updated successfully";

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
            TempData["error"] = "Error when updating Software Project";
            return View();
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

        
    }
}
