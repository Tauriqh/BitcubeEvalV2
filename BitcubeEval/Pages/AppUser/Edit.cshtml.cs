﻿using System.Linq;
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
    public class EditModel : PageModel
    {
        private readonly BitvalEvalContext _context;

        public EditModel(BitvalEvalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        static public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _context.ApplicationUser.FirstOrDefaultAsync(m => m.ID == id);

            if (ApplicationUser == null)
            {
                return NotFound();
            }

            Password = ApplicationUser.Password;

            return Page();
        }

        public async Task<IActionResult> OnPostSaveEditAsync()
        {
            ApplicationUser.Password = Password;

            _context.Attach(ApplicationUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(ApplicationUser.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Password = "";

            return RedirectToPage("/AppUser/Profile", new { Id = ApplicationUser.ID });
        }

        private bool ApplicationUserExists(int id)
        {
            return _context.ApplicationUser.Any(e => e.ID == id);
        }

        public RedirectToPageResult OnPostCancelEdit()
        {
            return RedirectToPage("/AppUser/Profile", new { Id = ApplicationUser.ID });
        }
    }
}
