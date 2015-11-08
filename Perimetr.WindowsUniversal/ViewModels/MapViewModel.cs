using GalaSoft.MvvmLight;
using Perimetr.WindowsUniversal.Messages;
using Perimetr.WindowsUniversal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Perimetr.WindowsUniversal.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private IFriendService friendService;
        private ObservableCollection<MapFriendViewModel> friends;

        public MapViewModel(IFriendService friendService)
        {
            this.friendService = friendService;
            this.friends = new ObservableCollection<MapFriendViewModel>();
            MessengerInstance.Register<GetMapPinsMessage>(this, async message => await GetFriendsAsync());
        }

        private async Task GetFriendsAsync()
        {
            var friends = await friendService.GetFriendsAsync();
            this.friends.Clear();
            foreach (var friend in friends)
            {
                this.friends.Add(new MapFriendViewModel
                {
                    FirstName = friend.FirstName,
                    Location = friend.Location.ToGeolocation()
                });
            }
        }

        public IList<MapFriendViewModel> Friends { get { return friends; } }

    }

    public class MapFriendViewModel
    {
        public string FirstName { get; set; }
        public Geopoint Location { get; set; }
    }

    public static class Extensions
    {
        public static Geopoint ToGeolocation(this LocationView location)
        {
            if (location != null)
            {
                return new Geopoint(new BasicGeoposition
                {
                    Altitude = location.Altitude,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                });
            }
            return null;
        }
    }
}
