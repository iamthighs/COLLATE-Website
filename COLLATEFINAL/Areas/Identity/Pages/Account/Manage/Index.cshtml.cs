// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using COLLATEFINAL.Data;
using COLLATEFINAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace COLLATEFINAL.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webhost;

        public IndexModel(
            UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager,
            IWebHostEnvironment webhost)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webhost = webhost;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        public string FirstName { get; set; }
          
        public string LastName { get; set; }

        public string ImageUrl { get; set; }
        public IFormFile? CoverImage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
        public class InputModel
        {
            
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Student Number")]
            public string StudentNumber { get; set; }

            public string NewFirstName { get; set; }
            public string NewLastName { get; set; }



        }

        private async Task LoadAsync(AppIdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            ImageUrl = user.ImageUrl;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                NewFirstName = user.FirstName,
                NewLastName = user.LastName

            };
        }

        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(user);
            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }



           

            string uniqueImg = UploadedFile(user);
            user.ImageUrl = uniqueImg;
            user.FirstName = Input.NewFirstName;
            user.LastName = Input.NewLastName;
            string imgext = Path.GetExtension(uniqueImg);
            if (imgext == ".jpg" || imgext == ".png")

            {

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["success"] = "Your Profile has been updated";
                }
                return RedirectToPage();

            }
            else
            {
                ModelState.AddModelError("", "Uploaded file is not a jpg or png file!");
                TempData["error"] = "Uploaded file is not a jpg or png file!";
            }
            return RedirectToPage();



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
    }
}
