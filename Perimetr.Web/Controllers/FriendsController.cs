using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Perimetr.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Perimetr.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Friends")]
    public class FriendsController : ApiController
    {
        private ApplicationUserManager _userManager;

        public FriendsController()
        {
        }


        public FriendsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<IHttpActionResult> Get()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var friends = user.Friends;
            var friendsViewModel = new List<FriendViewModels>();

            foreach (var friend in friends)
            {
                friendsViewModel.Add(new FriendViewModels
                {
                    Id = friend.Id,
                    Location = new LocationViewModel
                    {
                        Altitude = friend.Location.Altitude,
                        Latitude = friend.Location.Latitude,
                        Longitude = friend.Location.Longitude,
                        LastUpdated = friend.Location.LastUpdated
                    }
                });
            }

            return Json(friendsViewModel);
        }

        [Route("AddFriend")]
        public async Task<IHttpActionResult> AddFriend(string friendId)
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var friend = await UserManager.FindByIdAsync(friendId);

            if (friend != null)
            {
                user.Friends.Add(friend);

                return Ok();
            }

            return BadRequest("User does not exist");
        }

        [Route("FindFriends")]
        public async Task<IHttpActionResult> FindFriends(IEnumerable<ContactBindingModel> contacts)
        {
            var contactViewModels = new List<ContactViewModel>();
            foreach (var contact in contacts)
            {
                var possibleMatches = new List<PossibleFriendViewModel>();
                var users = await UserManager.Users.Where(
                    u =>
                    u.UserName == contact.Email &&
                    u.FirstName == contact.FirstName &&
                    u.LastName == contact.LastName
                    ).ToListAsync();

                possibleMatches.AddRange(users.Select(usr => new PossibleFriendViewModel
                {
                    Email = usr.Email,
                    FirstName = usr.FirstName,
                    LastName = usr.LastName,
                    Id = usr.Id
                }));

                var contactViewModel = new ContactViewModel
                {
                    Email = contact.Email,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName
                };
                contactViewModel.AddPossibleFriends(possibleMatches);
                contactViewModels.Add(contactViewModel);
            }

            return Json(contactViewModels);
        }
    }
}
