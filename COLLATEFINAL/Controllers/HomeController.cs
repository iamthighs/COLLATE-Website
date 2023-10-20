using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace COLLATEFINAL.Controllers
{
    
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager, IEmailSender emailSender, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
            
        }
        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Search(string title)
        {
            var resultsModel1 = _context.GameAndWebDevelopments.Where(item => item.Title.Contains(title)).ToList();
            var resultsModel2 = _context.ResearchPapers.Where(item => item.Title.Contains(title)).ToList();
            var resultsModel3 = _context.Events.Where(item => item.Title.Contains(title)).ToList();
            var resultsModel4 = _context.Subjects.Where(item => item.Subject.Contains(title)).ToList();
            // Pass the search results to the view
            var viewModel = new SearchViewModel
            {
                ResultsModel1 = resultsModel1,
                ResultsModel2 = resultsModel2,
                ResultsModel3 = resultsModel3,
                ResultsModel4 = resultsModel4
            };

            return View(viewModel);
        }

        


        [HttpGet]
        public IActionResult About()
        {
            FeedbackModel feedbackModel = new FeedbackModel();
            return View(feedbackModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult About(FeedbackModel model)
        {
            _emailSender.SendEmailAsync("collatewebsite@gmail.com", "Feedback from User",
                        $"From : <strong>{model.FullName}</strong> <br> {model.Feedback}");
            _context.Add(model);
            _context.SaveChanges();
            TempData["success"] = "Your feedback sent successfully. Thank you!";
            return View();
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}