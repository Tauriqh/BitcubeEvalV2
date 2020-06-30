using BitcubeEval.Data;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BitcubeEval.Pages.AppUser
{

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly BitvalEvalContext _context;

        public LoginModel(BitvalEvalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Remember email address?")]
        public bool RememberEmailAddress { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CookieEmailAddress { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public PageResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            if (string.IsNullOrEmpty(CookieEmailAddress))
            {
                // retriving the email address from the cookie if the user has previously checked on the remember email address check box
                CookieEmailAddress = Request.Cookies["EmailAddress"];
            }

            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Checks if the email and password match
            HashString Hash = new HashString();
            Input.Password = Hash.ComputeSha256Hash(Input.Password);

            var AppUser = _context.ApplicationUser
                .Where(u => u.EmailAddress == Input.Email && u.Password == Input.Password)
                .FirstOrDefault();

            if (AppUser == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            if (RememberEmailAddress == true)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30)
                };

                // saving the email addressin a cookie and when it expires
                Response.Cookies.Append("EmailAddress", Input.Email, cookieOptions);
            }

            await this.SignInUser(Input.Email, false);
            
            return RedirectToPage("/AppUser/Profile", new { Id = AppUser.ID });
        }

        private async Task SignInUser(string username, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                // Sign In.
                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { IsPersistent = isPersistent });
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }
    }
}