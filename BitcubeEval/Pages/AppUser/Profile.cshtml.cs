using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Authorization;
using BitcubeEval.Data;

namespace BitcubeEval.Pages.AppUser
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly BitvalEvalContext _context;

        public ProfileModel(BitvalEvalContext context)
        {
            _context = context;
        }

        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _context.ApplicationUser.FirstOrDefaultAsync(m => m.ID == Id);

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetLinkAsync(string Email)
        {
            if (Email == null)
            {
                return NotFound();
            }

            ApplicationUser = await _context.ApplicationUser.FirstOrDefaultAsync(m => m.EmailAddress == Email);

            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
