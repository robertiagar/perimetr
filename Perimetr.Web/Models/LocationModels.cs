using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Perimetr.Web.Models
{
    public class LocationModel
    {
        public string Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class GeofenceModel
    {
        public string Id { get; set; }
        public LocationModel Location { get; set; }
        public double Radius { get; set; }
    }
}