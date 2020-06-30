using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BitcubeEval.Data;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BitcubeEval.Pages.AppUser
{
    public class ChangePasswordModel : PageModel
    {
        private readonly BitvalEvalContext _context;

        public ChangePasswordModel(BitvalEvalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        static public string EmailAddress { get; set; }
        static public string FirstName { get; set; }
        static public string LastName { get; set; }

        static public string Password { get; set; }

        [BindProperty]
        [Display(Name = "Current Password")]
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CurrentPasswordErrorMessage { get; set; }

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

            EmailAddress = ApplicationUser.EmailAddress;
            FirstName = ApplicationUser.FirstName;
            LastName = ApplicationUser.LastName;
            Password = ApplicationUser.Password;

            return Page();
        }

        public async Task<IActionResult> OnPostSaveEditAsync()
        {
            ApplicationUser.EmailAddress = EmailAddress;
            ApplicationUser.FirstName = FirstName;
            ApplicationUser.LastName = LastName;

            HashString Hash = new HashString();
            CurrentPassword = Hash.ComputeSha256Hash(CurrentPassword);

            if (!CurrentPassword.Equals(Password))
            {
                CurrentPasswordErrorMessage = "Current password mismatch!!";
                return Page();
            }

            ApplicationUser.Password = Hash.ComputeSha256Hash(Password);

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

            EmailAddress = "";
            FirstName = "";
            LastName = "";

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