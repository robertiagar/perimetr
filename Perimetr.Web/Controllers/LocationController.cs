using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Perimetr.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private ApplicationUserManager userManager;
        private ApplicationDbContext dbContext;

        public LocationController()
        {
        }

        public LocationController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        public ApplicationDbContext DbContext
        {
            get { return dbContext ?? Request.GetOwinContext().Get<ApplicationDbContext>(); }
            private set { dbContext = value; }
        }

        [Route("UpdateLocation")]
        public async Task<IHttpActionResult> UpdateLocation(LocationBindingModel model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            
            int updateResult = -1;
            if (user.Location == null)
            {
                var location = new LocationModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Altitude = model.Altitude,
                    LastUpdated = DateTime.Now
                };

                DbContext.LocationModels.Add(location);

                updateResult = await DbContext.SaveChangesAsync();
            }
            else
            {
                user.Location.Altitude = model.Altitude;
                user.Location.LastUpdated = DateTime.Now;
                user.Location.Latitude = model.Latitude;
                user.Location.Longitude = model.Longitude;
                updateResult = await DbContext.SaveChangesAsync();
            }

            if (updateResult != 0)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}