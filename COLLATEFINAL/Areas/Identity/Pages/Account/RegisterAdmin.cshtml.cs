// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using COLLATEFINAL.Data;

namespace COLLATEFINAL.Areas.Identity.Pages.Account
{
    public class RegisterAdminModel : PageModel
    {
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IUserStore<AppIdentityUser> _userStore;
        private readonly IUserEmailStore<AppIdentityUser> _emailStore;
        private readonly ILogger<RegisterAdminModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webhost;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterAdminModel(
            UserManager<AppIdentityUser> userManager,
            IUserStore<AppIdentityUser> userStore,
            SignInManager<AppIdentityUser> signInManager,
            ILogger<RegisterAdminModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment webhost,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webhost = webhost;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }
        public IFormFile? CoverImage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            public Roles RoleName { get; set; }


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();



            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                string uniqueImg = UploadedFile(user);
                user.ImageUrl = uniqueImg;
                user.JoinedDate = DateTime.Now;
                string password = "P@ssw0rd";
                user.EmailConfirmed = true;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                string imgext = Path.GetExtension(uniqueImg);
                if (imgext == ".jpg" || imgext == ".png")
                {
                    var result = await _userManager.CreateAsync(user, password);



                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        _userManager.AddToRoleAsync(user, Input.RoleName.ToString()).Wait();

                        await _emailSender.SendEmailAsync(Input.Email, "You are added as New User",
                            $"Welcome to COLLATE {Input.FirstName} {Input.LastName}!");


                        return RedirectToAction("ListUsers", "Administration");

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        private string UploadedFile(AppIdentityUser user)
        {
            string uniqueFileName = user.ImageUrl;
            if (CoverImage != null)
            {
                string uploadsFolder = Path.Combine(_webhost.WebRootPath, "UserImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + CoverImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    CoverImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private AppIdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppIdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppIdentityUser)}'. " +
                    $"Ensure that '{nameof(AppIdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AppIdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppIdentityUser>)_userStore;
        }
    }
}
