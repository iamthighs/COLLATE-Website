using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace COLLATEWEBAPI.Controllers.Api
{
    [ApiController]
    [Route("collate/admin")]
    public class AdministrationApiController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;

        public AdministrationApiController(
            RoleManager<IdentityRole> roleManager,
            UserManager<AppIdentityUser> userManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHost)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _webHost = webHost;
        }

        // =========================
        // Roles Management
        // =========================

        [HttpGet("roles")]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToList();
            return Ok(roles);
        }

        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var role = new IdentityRole { Name = model.RoleName };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(role);
        }

        [HttpGet("roles/{roleId}")]
        public async Task<IActionResult> GetRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPut("roles/{roleId}")]
        public async Task<IActionResult> EditRole(string roleId, [FromBody] EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            role.Name = model.RoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(role);
        }

        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            try
            {
                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded) return BadRequest(result.Errors);
                return Ok(new { message = "Role deleted successfully" });
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { message = $"Cannot delete role '{role.Name}': {ex.Message}" });
            }
        }

        // =========================
        // Users Management
        // =========================

        [HttpGet("users")]
        public async Task<IActionResult> ListUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new { user.Id, user.UserName, user.Email, Roles = roles });
            }

            return Ok(result);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.ImageUrl,
                Roles = roles,
                Claims = claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpPut("users/{userId}")]
        public async Task<IActionResult> EditUser(string userId, [FromForm] EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (model.CoverImage != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "UserImages");
                string uniqueFileName = Guid.NewGuid() + "_" + model.CoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverImage.CopyToAsync(stream);
                }

                user.ImageUrl = uniqueFileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(user);
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = "User deleted successfully" });
        }

        // =========================
        // User Roles Management
        // =========================

        [HttpGet("users/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPut("users/{userId}/roles")]
        public async Task<IActionResult> UpdateUserRoles(string userId, [FromBody] List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded) return BadRequest(removeResult.Errors);

            var addResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addResult.Succeeded) return BadRequest(addResult.Errors);

            return Ok(new { message = "User roles updated successfully" });
        }

        // =========================
        // User Claims Management
        // =========================

        [HttpGet("users/{userId}/claims")]
        public async Task<IActionResult> GetUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var claims = await _userManager.GetClaimsAsync(user);
            return Ok(claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpPut("users/{userId}/claims")]
        public async Task<IActionResult> UpdateUserClaims(string userId, [FromBody] List<UserClaim> claims)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var existingClaims = await _userManager.GetClaimsAsync(user);
            var removeResult = await _userManager.RemoveClaimsAsync(user, existingClaims);
            if (!removeResult.Succeeded) return BadRequest(removeResult.Errors);

            var addResult = await _userManager.AddClaimsAsync(user,
                claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));
            if (!addResult.Succeeded) return BadRequest(addResult.Errors);

            return Ok(new { message = "User claims updated successfully" });
        }

        // =========================
        // Role Claims Management
        // =========================

        [HttpGet("roles/{roleId}/claims")]
        public async Task<IActionResult> GetRoleClaims(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            var claims = await _roleManager.GetClaimsAsync(role);
            return Ok(claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpPut("roles/{roleId}/claims")]
        public async Task<IActionResult> UpdateRoleClaims(string roleId, [FromBody] List<RoleClaim> claims)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            var existingClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in existingClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            foreach (var claim in claims.Where(c => c.IsSelected))
            {
                await _roleManager.AddClaimAsync(role, new Claim(claim.ClaimType, "Administrator"));
            }

            return Ok(new { message = "Role claims updated successfully" });
        }
    }
}
