﻿using BitcubeEval.Data;
using BitcubeEval.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcubeEval.Pages.AppUser
{
    [Authorize]
    public class FriendsModel : PageModel
    {
        private readonly BitvalEvalContext _context;

        public FriendsModel(BitvalEvalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public ApplicationUser UserFriend { get; set; }

        public Friend Friend { get; set; }

        public IList<ApplicationUser> ApplicationUserList { get; set; }

        public async Task<IActionResult> OnGetAsync(string Email)
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

            // join ApplicationUser and Friend table
            // get users where their ID in ApplicationUser table = UserID1 in Friend table
            var UsersWithIdEqualUserId1 = _context.ApplicationUser // ApplicationUser List
                .Join(_context.Friend, // join with Frined List
                    u => u.ID, // column for Application to join
                    f => f.UserID1, // column for Friend to join
                    (u, f) => new { ApplicationUser = u, Friend = f }); // select combination of application user and friend based on userId

            // join ApplicationUser and Friend table
            // get users where their ID in ApplicationUser table = UserID2 in Friend table
            var UsersWithIdEqualUserId2 = _context.ApplicationUser // ApplicationUser List
                .Join(_context.Friend, // join with Frined List
                    u => u.ID, // column for Application to join
                    f => f.UserID2, // column for Friend to join
                    (u, f) => new { ApplicationUser = u, Friend = f }); // select combination of application user and friend based on userId

            // return list where ApplicationUserID = userID2 and confirmed = true
            var friends = UsersWithIdEqualUserId1.Where(s => s.Friend.UserID2 == ApplicationUser.ID && s.Friend.Confirmed == true);
            // return list where ApplicationUserID = userID1 and confirmed = true
            var friends2 = UsersWithIdEqualUserId2.Where(s => s.Friend.UserID1 == ApplicationUser.ID && s.Friend.Confirmed == true);

            List<ApplicationUser> UserList = new List<ApplicationUser>();

            // add to UserList all the info of user where ApplicationUserID = userID2
            foreach (var item in friends)
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
            foreach (var item in friends2)
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

            ApplicationUserList = UserList;

            return Page();
        }

        public async Task<IActionResult> OnPostUnfriend(int? Id, int? FriendId)
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

            return RedirectToPage("/AppUser/Friends", new { Email = ApplicationUser.EmailAddress });
        }
    }
}