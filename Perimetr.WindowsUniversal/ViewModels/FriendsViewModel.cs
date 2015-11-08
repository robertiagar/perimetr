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
using GalaSoft.MvvmLight.Command;
using Perimetr.WindowsUniversal.Messages;
using System.Diagnostics;

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

            Messenger.Default.Register<GetFriendsMessage>(this, async message => await StartActionsAsync(message));
        }

        public IList<FriendView> Friends { get { return friends; } }
        public IList<ContactView> PossibleFriends { get { return possibleFriends; } }

        private async Task GetFriendsAsync()
        {
            var friends = await friendService.GetFriendsAsync();
            this.friends.Clear();
            foreach (var friend in friends)
            {
                this.friends.Add(friend);
            }
        }

        private async Task AddFreindAsync(string friendId)
        {
            await friendService.AddFriendAsync(friendId);
            await GetFriendsAsync();
        }

        private async Task FindFriendsAsync()
        {
            var contactStore = await ContactManager.RequestStoreAsync(ContactStoreAccessType.AllContactsReadOnly);
            var contacts = await contactStore.FindContactsAsync();

            Debug.WriteLine(contacts.Count);

            var contactBindings = contacts.Select(c => new ContactBinding
            {
                Email = c.Emails.FirstOrDefault() != null ? c.Emails.FirstOrDefault().Address : null,
                FirstName = c.FirstName,
                LastName = c.LastName
            }).ToList();

            Debug.WriteLine(contactBindings.Count);


            var contactViews = await friendService.FindFriendsAsync(contactBindings);
            possibleFriends.Clear();
            foreach (var contactView in contactViews)
            {
                foreach (var posibleFriend in contactView.PossibleFriends)
                {
                    posibleFriend.AddFriendCommand = new RelayCommand<string>(async str => await AddFreindAsync(str));
                }
                possibleFriends.Add(contactView);
            }
        }

        private async Task StartActionsAsync(GetFriendsMessage message)
        {
            await GetFriendsAsync();
            await FindFriendsAsync();
        }
    }
}
