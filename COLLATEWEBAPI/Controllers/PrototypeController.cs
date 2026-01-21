using COLLATE.Helpers.Common;
using COLLATE.Helpers.Data;
using COLLATE.Helpers.Models;
using COLLATE.Helpers.Helpers;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COLLATEWEBAPI.Controllers.Api
{
    [ApiController]
    [Route("collate/prototypes")]
    [Authorize(Roles = "Administrator,Faculty")]
    public class PrototypeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly FileHelper _file;


        public PrototypeApiController(ApplicationDbContext context, IWebHostEnvironment env, FileHelper file)
        {
            _context = context;
            _env = env;
            _file = file;
        }


        // ===================== LECTURES =====================

        [HttpGet("lectures")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLectures()
        {
            return Ok(await _context.Lectures.AsNoTracking().ToListAsync());
        }

        [HttpPost("lectures")]
        public async Task<IActionResult> CreateLecture([FromForm] LectureModel model)
        {
            model.FileUrl = await _file.SaveFileAsync(model.UploadedPDFFile, "PDF/Lectures");

            if (Path.GetExtension(model.FileUrl) != ".pdf")
                return BadRequest("Only PDF files are allowed");

            _context.Lectures.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("lectures/{id:int}")]
        public async Task<IActionResult> UpdateLecture(int id, [FromForm] LectureModel model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch");


            if (!await _context.Lectures.AnyAsync(x => x.Id == id))
                return NotFound();

            model.FileUrl = await _file.SaveFileAsync(model.UploadedPDFFile, "PDF/Lectures");

            _context.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("lectures/{id:int}")]
        public async Task<IActionResult> DeleteLecture(int id)
        {
            var entity = await _context.Lectures.FindAsync(id);
            if (entity == null) return NotFound();

            _file.DeleteFile("PDF/Lectures", entity.FileUrl);

            _context.Lectures.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ===================== VIDEOS =====================

        [HttpGet("videos")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVideos()
        {
            return Ok(await _context.Videos.AsNoTracking().ToListAsync());
        }

        [HttpPost("videos")]
        public async Task<IActionResult> CreateVideo([FromBody] VideosModel model)
        {
            _context.Videos.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("videos/{id:int}")]
        public async Task<IActionResult> UpdateVideo(int id, [FromBody] VideosModel model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch");

            if (!await _context.Videos.AnyAsync(x => x.Id == id))
                return NotFound();

            _context.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("videos/{id:int}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var entity = await _context.Videos.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Videos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
