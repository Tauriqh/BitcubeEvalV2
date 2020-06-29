using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Authorization;

namespace BitcubeEval.Pages.AppUser
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly BitcubeEval.Data.BitvalEvalContext _context;

        public IndexModel(BitcubeEval.Data.BitvalEvalContext context)
        {
            _context = context;
        }

        public IList<ApplicationUser> ApplicationUser { get;set; }

        public async Task OnGetAsync()
        {
            ApplicationUser = await _context.ApplicationUser.ToListAsync();
        }
    }
}
