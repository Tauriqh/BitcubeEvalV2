using System.Collections;
using System.Threading.Tasks;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BitcubeEval.Pages.AppUser
{
    public class LogoutModel : PageModel
    {
        public LogoutModel()
        {
            
        }

        public async Task<RedirectToPageResult> OnGetAsync()
        {
            var authenticationManager = Request.HttpContext;
            await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return this.RedirectToPage("/AppUser/Login");
        }
    }
}