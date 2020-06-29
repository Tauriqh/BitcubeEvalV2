using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BitcubeEval.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace BitcubeEval.Pages.AppUser
{
    [AllowAnonymous]
    public class CreateModel : PageModel
    {
        private readonly Data.BitvalEvalContext _context;

        public CreateModel(Data.BitvalEvalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ConfirmPasswordErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string EmailAddressErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            ConfirmPasswordErrorMessage = "";
            //EmailAddressErrorMessage = "";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ConfirmPasswordErrorMessage = "";
            EmailAddressErrorMessage = "";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!ConfirmPassword.Equals(ApplicationUser.Password))
            {
                ConfirmPasswordErrorMessage = "Passwords do not match!!";
                return Page();
            }

            try
            {
                // The Contains method is run on the database
                var email = (from u in _context.ApplicationUser
                             where (u.EmailAddress == ApplicationUser.EmailAddress)
                             select u.EmailAddress).Single();

                EmailAddressErrorMessage = "An account with this Email Address already exists!!";
                return Page();
            }
            catch (InvalidOperationException) //this error happens if the email address is unique
            {
                HashString Hash = new HashString();
                ApplicationUser.Password = Hash.ComputeSha256Hash(ApplicationUser.Password);

                _context.ApplicationUser.Add(ApplicationUser);
                await _context.SaveChangesAsync();

                await this.SignInUser(ApplicationUser.EmailAddress, false);

                return RedirectToPage("/Index");
            }
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
