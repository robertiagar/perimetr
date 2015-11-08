using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Perimetr.WindowsUniversal.Services
{
    public interface IFriendService
    {
        Task AddFriendAsync(string friendId);
        Task<IEnumerable<FriendView>> GetFriendsAsync();
        Task<IEnumerable<ContactView>> FindFriendsAsync(IEnumerable<ContactBinding> contacts);
    }

    public class ContactBinding
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class FriendView
    {
        public string FirstName { get; set; }
        public string FirstNameLetter { get { return FirstName[0].ToString(); } }
        public string LastName { get; set; }
        public string LastNameLetter { get { return LastName[0].ToString(); } }
        public string Email { get; set; }
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
        public string FirstNameLetter { get { return FirstName[0].ToString(); } }
        public string LastName { get; set; }
        public string LastNameLetter { get { return LastName[0].ToString(); } }
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
        public ICommand AddFriendCommand { get; set; }
    }
}
