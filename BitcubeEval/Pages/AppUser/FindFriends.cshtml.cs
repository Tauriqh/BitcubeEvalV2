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
    public class FindFriendsModel : PageModel
    {
        private readonly BitvalEvalContext _context;

        public FindFriendsModel(BitvalEvalContext context)
        {
            _context = context;
        }

        public ApplicationUser ApplicationUser { get; set; }

        public ApplicationUser UserFriend { get; set; }

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
            // get users where their ID in ApplicationUser table = UserID1 in Friend table
            var UsersWithIdEqualUserId1 = _context.ApplicationUser // ApplicationUser List
                .Join(_context.Friend, // join with Frined List
                    u => u.ID, // column for Application to join
                    f => f.UserID1, // column for Friend to join
                    (u, f) => new { ApplicationUser = u, Friend = f }); // select combination of u and f based on userId

            // join ApplicationUser and Friend table
            // get users where their ID in ApplicationUser table = UserID2 in Friend table
            var UsersWithIdEqualUserId2 = _context.ApplicationUser // ApplicationUser List
                .Join(_context.Friend, // join with Frined List
                    u => u.ID, // column for Application to join
                    f => f.UserID2, // column for Friend to join
                    (u, f) => new { ApplicationUser = u, Friend = f }); // select combination of u and f based on userId

            // return list where ApplicationUserID = userID2
            var usersAlreadyFriendOrRequested = UsersWithIdEqualUserId1.Where(s => s.Friend.UserID2 == ApplicationUser.ID);
            // return list where ApplicationUserID = userID1
            var usersAlreadyFriendOrRequested2 = UsersWithIdEqualUserId2.Where(s => s.Friend.UserID1 == ApplicationUser.ID);

            List <ApplicationUser> UserList = new List<ApplicationUser>();

            // add to UserList all the info of user where ApplicationUserID = userID2
            foreach (var item in usersAlreadyFriendOrRequested)
            {
                ApplicationUser user = new ApplicationUser
                {
                    ID = item.ApplicationUser.ID,
                    EmailAddress = item.ApplicationUser.EmailAddress,
                    FirstName = item.ApplicationUser.FirstName,
                    LastName = item.ApplicationUser.LastName
                };

                UserList.Add(user);
            }

            // add to UserList all the info of user where ApplicationUserID = userID1
            foreach (var item in usersAlreadyFriendOrRequested2)
            {
                ApplicationUser user = new ApplicationUser
                {
                    ID = item.ApplicationUser.ID,
                    EmailAddress = item.ApplicationUser.EmailAddress,
                    FirstName = item.ApplicationUser.FirstName,
                    LastName = item.ApplicationUser.LastName
                };

                UserList.Add(user);
            }

            // get list of all users
            IList<ApplicationUser> AllUsers = await _context.ApplicationUser.ToListAsync();
            IList<ApplicationUser> UsersIdList = new List<ApplicationUser>();
            
            // add to list all the user IDs where user ID != ApplicationUserID
            foreach (var item in AllUsers)
            {
                if (item.ID != ApplicationUser.ID)
                {
                    UsersIdList.Add(item);
                }
            }

            // return list of ApplicationUser info where friends of user(where current user ID = UserID1 & UserID12) their ID != any of the users ID
            // info on non friends
            ApplicationUserList = UsersIdList.Where(u => UserList.All(u2 => u2.ID != u.ID)).OrderBy(s => s.LastName).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostSendRequest(int? Id, int? FriendId)
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

            Friend Friend = new Friend
            {
                UserID1 = ApplicationUser.ID,
                UserID2 = UserFriend.ID,
                Confirmed = false
            };

            _context.Friend.Add(Friend);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AppUser/FindFriends", new { Id = ApplicationUser.ID });
        }
    }
}
