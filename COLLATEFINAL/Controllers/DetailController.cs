using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace COLLATEFINAL.Controllers
{
    [Authorize(Roles = "Administrator,Student,Faculty,sceneOfficer")]
    public class DetailController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppIdentityUser> userManager;

        public DetailController(ApplicationDbContext context, IWebHostEnvironment webHost, UserManager<AppIdentityUser> userManager)
        {
            _context = context;
            webHostEnvironment = webHost;
            this.userManager = userManager;
        }
        public IActionResult PrototypeDetail(int id, string pageType)
        {
            List<PrototypeModel> prototypeModels = _context.Prototypes.ToList();

            var detail = prototypeModels.FirstOrDefault(c => c.Id == id);
            ViewBag.PageType = pageType;

            return View(detail);
        }
        public IActionResult GameAndWebDevDetail(int id, string pageType)
        {
            List<GameAndWebDevModel> gameAndWebDevModels = _context.GameAndWebDevelopments.Include(p => p.Likes).Include(p => p.Comments).ToList();

            var detail = gameAndWebDevModels.FirstOrDefault(c => c.Id == id);
            ViewBag.PageType = pageType;

            // Retrieve data from the database
            var itemsFromDatabase = _context.GameAndWebDevelopments.ToList();

            var category = itemsFromDatabase.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.ImageUrl
            }).ToList();

            ViewBag.CountSoftware = _context.GameAndWebDevelopments.Count();
            ViewBag.pageType = pageType;
            ViewBag.category = category;

            return View(detail);
        }

        [HttpPost]
        public IActionResult Like(int postId)
        {
            // Get the current user ID
            string userId = User.Identity.Name;

            // Check if the user has already liked the post
            var existingLike = _context.PostLikes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);
            if (existingLike != null)
            {
                _context.PostLikes.Remove(existingLike);
                _context.SaveChanges();
                // User has already liked the post
                // You can handle this scenario based on your requirements
                return RedirectToAction(nameof(GameAndWebDevDetail), new { id = postId });
            }

            // Create a new like record
            var like = new Like
            {
                PostId = postId,
                UserId = userId
            };
            _context.PostLikes.Add(like);
            _context.SaveChanges();

            return RedirectToAction(nameof(GameAndWebDevDetail), new { id = postId });
        }

        [HttpPost]
        public IActionResult AddComment(int postId, string content)
        {
            // Get the current user ID

            string userId = User.Identity.Name;
            string imageUrl = "COLLATE.png";

            // Create a new comment record
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                CurrentDateTime = DateTime.Now,
                ImageUrl = imageUrl
            };
            _context.PostComments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction(nameof(GameAndWebDevDetail), new { id = postId });
        }

        public IActionResult ResearchPapersDetail(int id, string pageType)
        {
            List<ResearchPapersModel> researchPapersModels = _context.ResearchPapers.ToList();

            var detail = researchPapersModels.FirstOrDefault(c => c.Id == id);
            ViewBag.PageType = pageType;
            // Retrieve data from the database
            var itemsFromDatabase = _context.ResearchPapers.ToList();

            var category = itemsFromDatabase.Select(item => new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.ImageUrl
            }).ToList();


            ViewBag.pageType = pageType;
            ViewBag.category = category;
            return View(detail);
        }


        
        public IActionResult EventsDetail(int id, string pageType)
        {
            List<EventsModel> eventsModels = _context.Events.ToList();
            
            var detail = eventsModels.FirstOrDefault(c => c.Id == id);
            ViewBag.PageType = pageType;

            return View(detail);
        }

        


    }
}
