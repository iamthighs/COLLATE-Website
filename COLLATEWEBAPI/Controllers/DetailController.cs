using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COLLATEWEBAPI.Controllers.Api
{
    [ApiController]
    [Route("collate/details")]
    public class DetailApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppIdentityUser> _userManager;

        public DetailApiController(
            ApplicationDbContext context,
            UserManager<AppIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: api/details/research/{id}
        [HttpGet("research/{id}")]
        public async Task<IActionResult> GetResearchPaperDetail(int id)
        {
            var detail = await _context.ResearchPapers.FindAsync(id);
            if (detail == null) return NotFound();

            var category = await _context.ResearchPapers
                .Select(r => new { r.Id, r.ImageUrl })
                .ToListAsync();

            return Ok(new
            {
                detail,
                category,
                totalCount = await _context.ResearchPapers.CountAsync()
            });
        }

        // GET: api/details/software/{id}
        [HttpGet("software/{id}")]
        public async Task<IActionResult> GetSoftwareDetail(int id)
        {
            var detail = await _context.GameAndWebDevelopments
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (detail == null) return NotFound();

            var category = await _context.GameAndWebDevelopments
                .Select(p => new { p.Id, p.ImageUrl })
                .ToListAsync();

            return Ok(new
            {
                detail,
                category,
                totalCount = await _context.GameAndWebDevelopments.CountAsync()
            });
        }

        // POST: api/details/software/{postId}/like
        [HttpPost("software/{postId}/like")]
        public async Task<IActionResult> LikeSoftware(int postId)
        {
            var userId = User.Identity.Name;

            var existingLike = await _context.PostLikes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            if (existingLike != null)
            {
                _context.PostLikes.Remove(existingLike);
                await _context.SaveChangesAsync();
                return Ok(new { liked = false });
            }

            var like = new Like
            {
                PostId = postId,
                UserId = userId
            };
            _context.PostLikes.Add(like);
            await _context.SaveChangesAsync();

            return Ok(new { liked = true });
        }

        // POST: api/details/software/{postId}/comment
        [HttpPost("software/{postId}/comment")]
        public async Task<IActionResult> AddComment(int postId, [FromBody] string content)
        {
            var userId = User.Identity.Name;

            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                CurrentDateTime = DateTime.UtcNow,
                ImageUrl = "COLLATE.png"
            };

            _context.PostComments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        // GET: api/details/events/{id}
        [HttpGet("events/{id}")]
        public async Task<IActionResult> GetEventDetail(int id)
        {
            var detail = await _context.Events.FindAsync(id);
            if (detail == null) return NotFound();

            return Ok(detail);
        }
    }
}
