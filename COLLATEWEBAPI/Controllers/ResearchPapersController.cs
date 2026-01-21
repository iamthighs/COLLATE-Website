using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using COLLATEFINAL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace COLLATEWEBAPI.Controllers
{
    [ApiController]
    [Route("collate/research-papers")]
    public class ResearchPapersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileHelper _file;


        public ResearchPapersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, FileHelper file)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _file = file;
        }

        // GET: api/ResearchPapers
        // Optional query: pageNumber, pageSize, search
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ResearchPapersModel>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            // If you have a paginated helper on the context (used by your Razor pages), call it; otherwise return simple list.
            try
            {
                if (_context == null || _context.ResearchPapers == null)
                    return NotFound();

                // If your context exposes ResearchGetPaginated, keep using it (original controller used it in Index).
                // Fallback to simple filtering + pagination if it's not available at runtime.
                var query = _context.ResearchPapers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.Trim();
                    query = query.Where(r =>
                        (r.Title != null && r.Title.Contains(s)) ||
                        (r.Header != null && r.Header.Contains(s)) ||
                        (r.Description != null && r.Description.Contains(s)));
                }

                var items = await query
                    .OrderByDescending(r => r.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        // GET: api/ResearchPapers/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResearchPapersModel>> Get(int id)
        {
            if (_context.ResearchPapers == null)
                return NotFound();

            var researchPaper = await _context.ResearchPapers.FindAsync(id);
            if (researchPaper == null)
                return NotFound();

            return Ok(researchPaper);
        }

        // POST: api/ResearchPapers
        // Expects multipart/form-data with model fields plus files (CoverImage, UploadedCoverImage)
        [HttpPost]
        public async Task<ActionResult<ResearchPapersModel>> Post([FromForm] ResearchPapersModel researchPaperModel)
        {
            if (_context.ResearchPapers == null)
                return Problem("Entity set 'ApplicationDbContext.ResearchPapers' is null.");

            // Upload files first (if provided)
            try
            {
                var imgFileName = await _file.SaveFileAsync(researchPaperModel.CoverImage, "Uploads/ResearchPapers");
                var pdfFileName = await _file.SaveFileAsync(researchPaperModel.UploadedCoverImage, "PDF/ResearchPapers");

                if (researchPaperModel.CoverImage != null && string.IsNullOrEmpty(imgFileName))
                    return BadRequest("Invalid image upload. Allowed: .jpg, .png");

                if (researchPaperModel.UploadedCoverImage != null && string.IsNullOrEmpty(pdfFileName))
                    return BadRequest("Invalid PDF upload. Allowed: .pdf");

                if (!string.IsNullOrEmpty(imgFileName))
                    researchPaperModel.ImageUrl = imgFileName;

                if (!string.IsNullOrEmpty(pdfFileName))
                    researchPaperModel.FileUrl = pdfFileName;

                _context.ResearchPapers.Add(researchPaperModel);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = researchPaperModel.Id }, researchPaperModel);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        // PUT: api/ResearchPapers/5
        // Expects multipart/form-data for optional file updates
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] ResearchPapersModel researchPaperModel)
        {
            if (id != researchPaperModel.Id)
                return BadRequest("Id mismatch.");

            if (_context.ResearchPapers == null)
                return NotFound();

            var existing = await _context.ResearchPapers.FindAsync(id);
            if (existing == null)
                return NotFound();

            try
            {
                // Handle new uploads and delete old files if replaced
                if (researchPaperModel.CoverImage != null)
                {
                    var newImg = await _file.SaveFileAsync(researchPaperModel.CoverImage, "Uploads/ResearchPapers");
                    if (string.IsNullOrEmpty(newImg))
                        return BadRequest("Invalid image upload. Allowed: .jpg, .png");

                    // delete old image file if exists
                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        _file.DeleteFile("Uploads/ResearchPapers", existing.ImageUrl);
                    }
                    existing.ImageUrl = newImg;
                }

                if (researchPaperModel.UploadedCoverImage != null)
                {
                    var newPdf = await _file.SaveFileAsync(researchPaperModel.UploadedCoverImage, "PDF/ResearchPapers");
                    if (string.IsNullOrEmpty(newPdf))
                        return BadRequest("Invalid PDF upload. Allowed: .pdf");

                    // delete old pdf if exists
                    if (!string.IsNullOrEmpty(existing.FileUrl))
                    {
                        _file.DeleteFile("PDF/ResearchPapers", existing.FileUrl);
                    }
                    existing.FileUrl = newPdf;
                }

                // Update scalar properties - copy values from incoming model to existing entity
                // If your model has many properties consider a mapping library; below uses simple assignment for common fields.
                existing.Title = researchPaperModel.Title;
                existing.Description = researchPaperModel.Description;
                existing.Authors = researchPaperModel.Authors;
                existing.YearSec = researchPaperModel.YearSec;
                existing.PostedDate = researchPaperModel.PostedDate;
                existing.Header = researchPaperModel.Header;
                // Add other property assignments as required by your ResearchPapersModel

                _context.Update(existing);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResearchPaperExists(id))
                    return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }
        }

        // DELETE: api/ResearchPapers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.ResearchPapers == null)
                return NotFound();

            var researchPaper = await _context.ResearchPapers.FindAsync(id);
            if (researchPaper == null)
                return NotFound();

            // remove files
            if (!string.IsNullOrEmpty(researchPaper.FileUrl))
            {
                _file.DeleteFile("PDF/ResearchPapers", researchPaper.FileUrl);
            }

            if (!string.IsNullOrEmpty(researchPaper.ImageUrl))
            {
                _file.DeleteFile("Uploads/ResearchPapers", researchPaper.ImageUrl);
            }

            _context.ResearchPapers.Remove(researchPaper);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResearchPaperExists(int id)
        {
            return (_context.ResearchPapers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}