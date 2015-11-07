using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perimetr.WindowsUniversal.Services
{
    public interface IFriendService
    {
        Task AddFriendAsync(string friendId);
        Task<IEnumerable<FriendView>> GetFriendsAsync();
        Task<IEnumerable<ContactView>> FindFriendAsync(IEnumerable<ContactBinding> contacts);
    }

    public class ContactBinding
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class FriendView
    {
        public string Id { get; set; }
        public LocationView Location { get; set; }
    }

    public class LocationView
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class ContactView
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<PossibleFriendView> PossibleFriends { get; private set; }

        public ContactView()
        {
            this.PossibleFriends = new List<PossibleFriendView>();
        }

        internal void AddPossibleFriends(IEnumerable<PossibleFriendView> possibleMatches)
        {
            (PossibleFriends as List<PossibleFriendView>).AddRange(possibleMatches);
        }
    }

    public class PossibleFriendView
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
