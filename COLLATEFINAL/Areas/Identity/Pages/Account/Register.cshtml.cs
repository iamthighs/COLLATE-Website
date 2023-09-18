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
using COLLATEFINAL.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace COLLATEFINAL.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IUserStore<AppIdentityUser> _userStore;
        private readonly IUserEmailStore<AppIdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webhost;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RegisterModel(
            UserManager<AppIdentityUser> userManager,
            IUserStore<AppIdentityUser> userStore,
            SignInManager<AppIdentityUser> signInManager,
            ILogger<RegisterModel> logger,
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
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string First { get; set; }

            

            [Required]
            [Display(Name = "Last Name")]
            public string Last { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ImageUrl, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var saveimg = Path.Combine(_webhost.WebRootPath, "UserImages", ImageUrl.FileName);


            if (saveimg == null || saveimg.Length == 0 || ImageUrl.FileName == null || ImageUrl.FileName.Length == 0)
            {

                ModelState.AddModelError("", "Uploaded file is empty or null.");
            }

            string imgext = Path.GetExtension(ImageUrl.FileName);



            if (imgext == ".jpg" || imgext == ".png")

            {


                using var uploadimg = new FileStream(saveimg, FileMode.Create);

                await ImageUrl.CopyToAsync(uploadimg);
               

            }
            else
            {
                ModelState.AddModelError("", "Uploaded file is not a jpg or png file!");
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.LastName = Input.Last;
                user.FirstName = Input.First;
                user.ImageUrl = ImageUrl.FileName;


                //await _userStore.SetUserNameAsync(user, Input.First, CancellationToken.None);

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (Input.Email == "collatewebsite@gmail.com") 
                    {
                        user.EmailConfirmed = true;
                        user.PhoneNumberConfirmed = true;
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Student.ToString()));
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Faculty.ToString()));
                        await _roleManager.CreateAsync(new IdentityRole(Roles.sceneOfficer.ToString()));


                        _userManager.AddToRoleAsync(user, Roles.Administrator.ToString()).Wait();
                    }
                    _userManager.AddToRoleAsync(user, Roles.Student.ToString()).Wait();

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync("collatewebsite@gmail.com", "Newly Registered User",
                        $"A new user named <strong>{Input.First} {Input.Last}</strong>  has just signed up, Please confirm this account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here to notify the user</a>.");

                    await _emailSender.SendEmailAsync(Input.Email, "COLLATE Website",
                        $"Thank you for creating an account. Please wait for the administrator's approval.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
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
