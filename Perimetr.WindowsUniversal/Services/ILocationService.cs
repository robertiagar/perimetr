using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perimetr.WindowsUniversal.Services
{
    public interface ILocationService
    {
        Task UpdateLocationAsync(LocationBinding location);
    }

    public class LocationBinding
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}
