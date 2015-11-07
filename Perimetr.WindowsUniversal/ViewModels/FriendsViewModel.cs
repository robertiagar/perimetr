using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Perimetr.WindowsUniversal.Services;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Contacts;

namespace Perimetr.WindowsUniversal.ViewModels
{
    public class FriendsViewModel : ViewModelBase
    {
        private IFriendService friendService;

        private ObservableCollection<FriendView> friends;
        private ObservableCollection<ContactView> possibleFriends;

        public FriendsViewModel(IFriendService friendService)
        {
            this.friendService = friendService;
            this.friends = new ObservableCollection<FriendView>();
            this.possibleFriends = new ObservableCollection<ContactView>();
        }

        public ICommand GetFriendsCommand { get; private set; }
        public ICommand FindFriendsCommand { get; private set; }
        public ICommand AddFriendCommand { get; private set; }

        public async Task GetFriendsAsync()
        {
            var friends = await friendService.GetFriendsAsync();
            foreach (var friend in friends)
            {
                this.friends.Add(friend);
            }
        }

        public async Task FindFriendsAsync()
        {
            var contactStore = await ContactManager.RequestStoreAsync();
            var contacts = await contactStore.FindContactsAsync();

            var contactBindings = contacts.Select(c => new ContactBinding
            {
                Email = c.Emails.FirstOrDefault().Address,
                FirstName = c.FirstName,
                LastName = c.LastName
            });

            var contactViews = await friendService.FindFriendsAsync(contactBindings);
            foreach (var contactView in contactViews)
            {
                possibleFriends.Add(contactView);
            }
        }
    }
}
