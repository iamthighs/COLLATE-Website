using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COLLATEWEBAPI.Controllers.Api
{
    [ApiController]
    [Route("collate/dashboard")]
    public class DashboardApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardApiController(
            ApplicationDbContext context,
            UserManager<AppIdentityUser> userManager,
            ILogger<DashboardApiController> logger)
        {
            _context = context;
        }

        /// <summary>
        /// Dashboard summary counts
        /// </summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = new
            {
                prototypes = await _context.Prototypes.CountAsync(),
                software = await _context.GameAndWebDevelopments.CountAsync(),
                research = await _context.ResearchPapers.CountAsync(),
                users = await _context.Users.CountAsync(),
                roles = await _context.Roles.CountAsync(),
                events = await _context.Events.CountAsync(),
                subjects = await _context.Subjects.CountAsync(),
                lectures = await _context.Lectures.CountAsync(),
                videos = await _context.Videos.CountAsync()
            };

            return Ok(result);
        }
    }
}
