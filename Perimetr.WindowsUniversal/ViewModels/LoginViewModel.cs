using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Perimetr.WindowsUniversal.Messages;
using Perimetr.WindowsUniversal.Services;
using Perimetr.WindowsUniversal.Views;
using System.Diagnostics;
using System.Windows.Input;

namespace Perimetr.WindowsUniversal.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string username;
        private string password;

        private ILoginService loginService;
        private ISettingsService settingsService;

        public LoginViewModel(ILoginService loginService, ISettingsService settingsService)
        {
            this.loginService = loginService;
            this.settingsService = settingsService;
            this.LoginCommand = new RelayCommand(async () =>
            {
                Debug.WriteLine("Login...");
                var token = await loginService.GetAccessTokenAsync(Username, Password);
                await this.settingsService.SaveItemAsync(token, "access_token");

                Messenger.Default.Send(new NavigationMessage(typeof(FriendsPage), null));
            });
        }

        public ICommand LoginCommand { get; private set; }

        public string Username
        {
            get { return username; }
            set
            {
                Set(nameof(Username), ref username, value);
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                Set(nameof(Password), ref password, value);
            }
        }
    }
}