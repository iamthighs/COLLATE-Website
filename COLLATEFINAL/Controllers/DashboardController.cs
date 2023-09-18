using COLLATEFINAL.Common;
using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace COLLATEFINAL.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppIdentityUser> userManager;


        public DashboardController(ILogger<DashboardController> logger, ApplicationDbContext context, UserManager<AppIdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            this.userManager = userManager;

        }

        public IActionResult Index()
        {
            
            ViewBag.CountProto = _context.Prototypes.Count();
            ViewBag.CountSoftware = _context.GameAndWebDevelopments.Count();
            ViewBag.CountResearch = _context.ResearchPapers.Count();
            ViewBag.CountUsers = _context.Users.Count();
            ViewBag.CountRoles = _context.Roles.Count();
            ViewBag.CountEvents = _context.Events.Count();
            ViewBag.CountSubj = _context.Subjects.Count();
            ViewBag.CountLec = _context.Lectures.Count();
            ViewBag.CountVid = _context.Videos.Count();

            return View();
        }


        


        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}