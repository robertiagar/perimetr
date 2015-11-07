using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Perimetr.WindowsUniversal.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace Perimetr.WindowsUniversal.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string username;
        private string password;
        private ILoginService loging;

        public LoginViewModel(ILoginService login)
        {
            this.loging = login;
            this.LoginCommand = new RelayCommand(async () =>
            {
                Debug.WriteLine("Login...");
                var t = await login.GetAccessTokenAsync(Username, Password);
            });
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

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