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
using COLLATEFINAL.Models;
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
    public class AboutModel : PageModel
    {
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IUserStore<AppIdentityUser> _userStore;
        private readonly IUserEmailStore<AppIdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webhost;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AboutModel(
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

        
        [BindProperty]
        public InputModel Input { get; set; }

        
        public class InputModel
        {
            
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [Display(Name = "Feedback")]
            public string Feedback { get; set; }

            
        }


        public async Task OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            await _emailSender.SendEmailAsync("collatewebsite@gmail.com", "Feedback from User",
                        $"From : <strong>{Input.FullName}</strong> <br> {Input.Feedback}");
            TempData["success"] = "Your feedback sent successfully. Thank you!";
            return Page();
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
