using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Perimetr.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Perimetr.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Location")]
    public class LocationController : ApiController
    {
        private ApplicationUserManager _userManager;

        public LocationController(ApplicationUserManager userManager)
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

        [Route("UpdateLocation")]
        public async Task<IHttpActionResult> UpdateLocation(LocationBindingModel model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.Location = new LocationModel
            {
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Altitude = model.Altitude,
                LastUpdated = DateTime.Now
            };

            var updateResult = await UserManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return Ok();
            }

            return InternalServerError(new Exception(string.Join("\n\r", updateResult.Errors.ToArray())));
        }
    }
}