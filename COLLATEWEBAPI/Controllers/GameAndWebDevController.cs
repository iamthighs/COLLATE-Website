using COLLATEFINAL.Common;
using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using COLLATEFINAL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COLLATEWEBAPI.Controllers.Api
{
    [ApiController]
    [Route("collate/game-web-dev")]
    public class GameAndWebDevApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly FileHelper _file;


        public GameAndWebDevApiController(
            ApplicationDbContext context,
            IWebHostEnvironment env, FileHelper file)
        {
            _context = context;
            _env = env;
            _file = file;
        }

        // GET: api/game-web-dev
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GameAndWebDevModel>>> GetAll()
        {
            var items = await _context.GameAndWebDevelopments
                .OrderBy(x => x.Title)
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/game-web-dev/paged
        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetPaged([FromQuery] PaginatedRequest request)
        {
            var result = await _context.SoftwareGetPaginated(
                request.PageNumber,
                PaginatedRequest.ITEMS_PER_PAGE,
                request.SearchKeyword ?? string.Empty);

            return Ok(result);
        }

        // GET: api/game-web-dev/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GameAndWebDevModel>> GetById(int id)
        {
            var item = await _context.GameAndWebDevelopments.FindAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/game-web-dev
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Create([FromForm] GameAndWebDevCreateDto dto)
        {
            if (!_file.IsValidImage(dto.CoverImage))
                return BadRequest("Only .jpg and .png files are allowed.");

            var imageUrl = await _file.SaveFileAsync(dto.CoverImage, "Uploads/SoftwareProjects");

            var model = new GameAndWebDevModel
            {
                Title = dto.Title,
                Description = dto.Description,
                YearSec = dto.YearAndSection,
                ImageUrl = imageUrl
            };

            _context.GameAndWebDevelopments.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: api/game-web-dev/{id}
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(int id, [FromForm] GameAndWebDevUpdateDto dto)
        {
            var entity = await _context.GameAndWebDevelopments.FindAsync(id);
            if (entity == null)
                return NotFound();

            if (dto.CoverImage != null)
            {
                if (!_file.IsValidImage(dto.CoverImage))
                    return BadRequest("Only .jpg and .png files are allowed.");

                _file.DeleteFile("Uploads/SoftwareProjects", entity.ImageUrl);
                entity.ImageUrl = await _file.SaveFileAsync(dto.CoverImage, "Uploads/SoftwareProjects");
            }

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.YearSec = dto.YearAndSection;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/game-web-dev/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _context.GameAndWebDevelopments.FindAsync(id);
            if (entity == null)
                return NotFound();

            _file.DeleteFile("Uploads/SoftwareProjects", entity.ImageUrl);

            _context.GameAndWebDevelopments.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
