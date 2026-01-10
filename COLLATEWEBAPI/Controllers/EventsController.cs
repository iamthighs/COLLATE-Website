using COLLATEFINAL.Common;
using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using COLLATEFINAL.ViewModels;
using COLLATEWEBAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COLLATEWEBAPI.Controllers.Api
{
    [ApiController]
    [Route("collate/events")]
    public class EventsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly FileHelper _file;


        public EventsApiController(
            ApplicationDbContext context,
            IWebHostEnvironment env,
            UserManager<AppIdentityUser> userManager, FileHelper file)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _file = file;
        }

        // -------------------- Events --------------------

        // GET: api/events
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.Events
                .Include(e => e.Attendees)
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/events/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            return ev == null ? NotFound() : Ok(ev);
        }

        // GET: api/events/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginatedRequest request)
        {
            var result = await _context.EventsGetPaginated(
                request.PageNumber,
                PaginatedRequest.ITEMS_PER_PAGE,
                request.SearchKeyword ?? string.Empty);

            return Ok(result);
        }

        // POST: api/events
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] EventsCreateDto dto)
        {
            if (!_file.IsValidImage(dto.CoverImage))
                return BadRequest("Only .jpg and .png files are allowed.");

            var imageUrl = await _file.SaveFileAsync(dto.CoverImage, "Uploads/Events");

            var ev = new EventsModel
            {
                Title = dto.Title,
                Objectives = dto.Description,
                Category = dto.Category,
                PostedDate = dto.EventDate,
                ImageUrl = imageUrl
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = ev.Id }, ev);
        }

        // PUT: api/events/{id}
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] EventsUpdateDto dto)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();

            if (dto.CoverImage != null)
            {
                if (!_file.IsValidImage(dto.CoverImage))
                    return BadRequest("Only .jpg and .png files are allowed.");

                _file.DeleteFile("Uploads/Events", ev.ImageUrl);
                ev.ImageUrl = await _file.SaveFileAsync(dto.CoverImage, "Uploads/Events");
            }

            ev.Title = dto.Title;
            ev.Objectives = dto.Description;
            ev.Category = dto.Category;
            ev.PostedDate = dto.EventDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/events/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();

            _file.DeleteFile("Uploads/Events", ev.ImageUrl);
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // -------------------- Registration --------------------

        // POST: api/events/{id}/register
        [HttpPost("{id:int}/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            var userId = _userManager.GetUserId(User);
            var user = userId == null ? null : await _userManager.FindByIdAsync(userId);

            if (ev == null || user == null)
                return NotFound();

            if (ev.Attendees.Contains(user))
                return Conflict("User already registered for this event.");

            ev.Attendees.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Successfully registered for the event.");
        }

        // POST: api/events/{id}/attendees/{userId}
        [HttpPost("{id:int}/attendees/{userId}")]
        public async Task<IActionResult> AddUser(int id, string userId)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            var user = await _userManager.FindByIdAsync(userId);

            if (ev == null || user == null)
                return NotFound();

            if (!ev.Attendees.Contains(user))
            {
                ev.Attendees.Add(user);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        // DELETE: api/events/{id}/attendees/{userId}
        [HttpDelete("{id:int}/attendees/{userId}")]
        public async Task<IActionResult> RemoveUser(int id, string userId)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            var user = await _userManager.FindByIdAsync(userId);

            if (ev == null || user == null)
                return NotFound();

            if (ev.Attendees.Contains(user))
            {
                ev.Attendees.Remove(user);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

    }
}
