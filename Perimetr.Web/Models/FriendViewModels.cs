using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Perimetr.Web.Models
{
    public class PossibleFriendViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class ContactViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<PossibleFriendViewModel> PossibleFriends { get; private set; }

        public ContactViewModel()
        {
            this.PossibleFriends = new List<PossibleFriendViewModel>();
        }

        internal void AddPossibleFriends(IEnumerable<PossibleFriendViewModel> possibleMatches)
        {
            (PossibleFriends as List<PossibleFriendViewModel>).AddRange(possibleMatches);
        }
    }
}