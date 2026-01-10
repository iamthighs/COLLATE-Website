using COLLATEFINAL.Common;
using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COLLATEWEBAPI.Helpers;

namespace COLLATEWEBAPI.Controllers.Api
{
    [ApiController]
    [Route("collate/subjects")]
    public class SubjectsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly FileHelper _file;

        public SubjectsApiController(
            ApplicationDbContext context,
            IWebHostEnvironment env,
            FileHelper file)
        {
            _context = context;
            _env = env;
            _file = file;
        }

        // -------------------- Subjects --------------------

        // GET: api/subjects
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return Ok(subjects);
        }

        // GET: api/subjects/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginatedRequest request)
        {
            var result = await _context.SubjectsGetPaginated(
                request.PageNumber,
                PaginatedRequest.ITEMS_PER_PAGE,
                request.SearchKeyword ?? string.Empty);

            return Ok(result);
        }

        // GET: api/subjects/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            return subject == null ? NotFound() : Ok(subject);
        }

        // POST: api/subjects
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] SubjectCreateDto dto)
        {
            if (!_file.IsValidImage(dto.CoverImage))
                return BadRequest("Only .jpg and .png files are allowed.");

            var imageUrl = await _file.SaveFileAsync(dto.CoverImage, "Uploads/Subjects");

            var subject = new SubjectModel
            {
                Subject = dto.Subject,
                ImageUrl = imageUrl
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = subject.Id }, subject);
        }

        // PUT: api/subjects/{id}
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] SubjectUpdateDto dto)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound();

            if (dto.CoverImage != null)
            {
                if (!_file.IsValidImage(dto.CoverImage))
                    return BadRequest("Only .jpg and .png files are allowed.");

                _file.DeleteFile("Uploads/Subjects", subject.ImageUrl);
                subject.ImageUrl = await _file.SaveFileAsync(dto.CoverImage, "Uploads/Subjects");
            }

            subject.Subject = dto.Subject;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/subjects/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound();

            _file.DeleteFile("Uploads/Subjects" , subject.ImageUrl);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // -------------------- Subject Details --------------------

        // GET: api/subjects/{id}/details
        [HttpGet("{id:int}/details")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetails(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound();

            var model = new EditSubjViewModel
            {
                Id = subject.Id,
                Subject = subject.Subject,
                Lectures = await _context.Lectures
                    .Where(l => l.Subject == subject.Subject)
                    .ToListAsync(),
                Videos = await _context.Videos
                    .Where(v => v.Subject == subject.Subject)
                    .ToListAsync()
            };

            return Ok(model);
        }

        // -------------------- Lectures Assignment --------------------

        // PUT: api/subjects/{id}/lectures
        [HttpPut("{id:int}/lectures")]
        public async Task<IActionResult> UpdateLectures(
            int id,
            [FromBody] List<LecSubjViewModel> model)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound();

            foreach (var item in model)
            {
                var lecture = await _context.Lectures.FindAsync(item.LecId);
                if (lecture == null)
                    continue;

                lecture.Subject = item.IsSelected ? subject.Subject : null;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        
    }
}
