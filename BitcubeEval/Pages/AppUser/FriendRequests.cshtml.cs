using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitcubeEval.Data;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BitcubeEval.Pages.AppUser
{
    [Authorize]
    public class FriendRequestsModel : PageModel
    {
        private readonly BitvalEvalContext _context;

        public FriendRequestsModel(BitvalEvalContext context)
        {
            _context = context;
        }

        public ApplicationUser ApplicationUser { get; set; }

        public ApplicationUser UserFriend { get; set; }

        public  Friend Friend { get; set; }

        public IList<ApplicationUser> ApplicationUserList { get; set; }

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

            // join ApplicationUser and Friend table
            var users = _context.ApplicationUser // ApplicationUser List
                .Join(_context.Friend, // join with Frined List
                    u => u.ID, // column for Application to join
                    f => f.UserID1, // column for Friend to join
                    (u, f) => new { ApplicationUser = u, Friend = f }); // select combination of u and f based on userId

            var friends = users.Where(s => s.Friend.UserID2 == ApplicationUser.ID && s.Friend.Confirmed == false);

            List<ApplicationUser> UserList = new List<ApplicationUser>();

            foreach (var item in friends)
            {
                ApplicationUser user = new ApplicationUser();
                user.ID = item.ApplicationUser.ID;
                user.EmailAddress = item.ApplicationUser.EmailAddress;
                user.FirstName = item.ApplicationUser.FirstName;
                user.LastName = item.ApplicationUser.LastName;

                UserList.Add(user);
            }

            ApplicationUserList = UserList;

            return Page();
        }

        public async Task<IActionResult> OnPostAccept(int? Id, int? FriendId)
        {
            if (Id == null || FriendId == null)
            {
                return NotFound();
            }

            ApplicationUser = await _context.ApplicationUser.FirstOrDefaultAsync(m => m.ID == Id);
            UserFriend = await _context.ApplicationUser.FirstOrDefaultAsync(x => x.ID == FriendId);

            if (ApplicationUser == null || UserFriend == null)
            {
                return NotFound();
            }

            Friend = await _context.Friend.FirstOrDefaultAsync(x => x.UserID1 == ApplicationUser.ID && x.UserID2 == UserFriend.ID);

            if (Friend == null)
            {
                Friend = await _context.Friend.FirstOrDefaultAsync(x => x.UserID2 == ApplicationUser.ID && x.UserID1 == UserFriend.ID);
                if (Friend == null)
                {
                    return NotFound();
                }  
            }

            Friend.Confirmed = true;

            _context.Attach(Friend).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(Friend.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/AppUser/FriendRequests", new { Id = ApplicationUser.ID });
        }

        private bool FriendExists(int id)
        {
            return _context.Friend.Any(e => e.ID == id);
        }

        public async Task<IActionResult> OnPostDecline(int? Id, int? FriendId)
        {
            if (Id == null || FriendId == null)
            {
                return NotFound();
            }

            ApplicationUser = await _context.ApplicationUser.FirstOrDefaultAsync(m => m.ID == Id);
            UserFriend = await _context.ApplicationUser.FirstOrDefaultAsync(x => x.ID == FriendId);

            if (ApplicationUser == null || UserFriend == null)
            {
                return NotFound();
            }

            Friend = await _context.Friend.FirstOrDefaultAsync(x => x.UserID1 == ApplicationUser.ID && x.UserID2 == UserFriend.ID);

            if (Friend == null)
            {
                Friend = await _context.Friend.FirstOrDefaultAsync(x => x.UserID2 == ApplicationUser.ID && x.UserID1 == UserFriend.ID);

                if (Friend == null)
                {
                    return NotFound();
                }
            }
            
            _context.Friend.Remove(Friend);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AppUser/FriendRequests", new { Id = ApplicationUser.ID });
        }
    }
}