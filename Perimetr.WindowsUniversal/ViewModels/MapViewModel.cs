using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Perimetr.WindowsUniversal.Messages;
using Perimetr.WindowsUniversal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace Perimetr.WindowsUniversal.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private IFriendService friendService;
        private ObservableCollection<MapFriendViewModel> friends;
        private Geolocator _geolocator;
        private MeViewModel me;
        private MapFriendViewModel je;

        public MapViewModel(IFriendService friendService)
        {
            this.friendService = friendService;
            this.friends = new ObservableCollection<MapFriendViewModel>();
            this.me = new MeViewModel();
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
                    Id = friend.Id,
                    FirstName = friend.FirstName,
                    LastName = friend.LastName,
                    LastUpdated = friend.Location != null ? friend.Location.LastUpdated : DateTime.MinValue,
                    Location = friend.Location.ToGeolocation()
                });
            }

            AddMe();

            var geofenceMessage = new SetupGeofencingMessage(this.friends);
            Messenger.Default.Send(geofenceMessage);
        }

        private void AddMe()
        {
            _geolocator = new Geolocator { ReportInterval = 2000 };

            // Subscribe to PositionChanged event to get updated tracking positions
            _geolocator.PositionChanged += OnPositionChanged;

            // Subscribe to StatusChanged event to get updates of location status changes
            _geolocator.StatusChanged += OnStatusChanged;
        }

        private void OnStatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
        }

        private async void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var position = await sender.GetGeopositionAsync();
            me.Location = position;
            if (this.je != null)
            {
                await DispatcherHelper.RunAsync(() =>
                {
                    friends.Remove(je);
                });

                this.je = new MapFriendViewModel
                {
                    FirstName = "Me",
                    LastName = "Me",
                    Location = me.Location.Coordinate.Point,
                    LastUpdated = DateTime.Now
                };

                await DispatcherHelper.RunAsync(() =>
                {
                    friends.Add(this.je);
                });
            }
            else
            {
                this.je = new MapFriendViewModel
                {
                    FirstName = "Me",
                    LastName = "Me",
                    Location = me.Location.Coordinate.Point,
                    LastUpdated = DateTime.Now
                };

                await DispatcherHelper.RunAsync(() =>
                {
                    friends.Add(this.je);
                });
            }
        }

        public IList<MapFriendViewModel> Friends { get { return friends; } }

    }

    public class MapFriendViewModel
    {
        public string FirstName { get; set; }
        public string FirstNameLetter { get { return FirstName[0].ToString(); } }

        public string Id { get; set; }
        public string LastName { get; set; }
        public string LastNameLetter { get { return LastName[0].ToString(); } }
        public DateTime LastUpdated { get; set; }
        public Geopoint Location { get; set; }
    }

    public class MeViewModel : ObservableObject
    {
        private Geoposition location;
        public Geoposition Location
        {
            get { return location; }
            set
            {
                Set(nameof(Location), ref location, value);
            }
        }
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
