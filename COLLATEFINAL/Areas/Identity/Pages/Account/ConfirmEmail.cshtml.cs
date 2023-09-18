// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COLLATEFINAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace COLLATEFINAL.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ConfirmEmailModel(UserManager<AppIdentityUser> userManager, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            //var inputEmail = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with an ID '{userId}'.");
            }

            
            //if (inputEmail == null)
            //{
            //    return NotFound($"Unable to load user with email '{email}'.");
            //}


            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            

            StatusMessage = result.Succeeded ? "Done confirming this email." : "Error confirming this email.";
            await _emailSender.SendEmailAsync(user.Email, "COLLATE Website",
                       $"Congratulations <strong>{user.FirstName}</strong>! The Administrator approved your registration. You may now log in");
            return Page();
        }
    }
}
