﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BitcubeEval.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        /*public async Task<IActionResult> OnPostLogOff()
        {
           
            var authenticationManager = Request.HttpContext;

            // Sign Out.  
            await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           
            // Info.  
            return this.RedirectToPage("/Index");
        }*/
    }
}