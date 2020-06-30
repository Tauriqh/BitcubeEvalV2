using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BitcubeEval.Pages.AppUser
{
    public class FriendsModel : PageModel
    {
        public PageResult OnGet()
        {
            return Page();
        }
    }
}