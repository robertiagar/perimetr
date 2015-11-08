using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Perimetr.WindowsUniversal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perimetr.WindowsUniversal.ViewModels
{
    class ViewModelLocator
    {
        private readonly string baseUrl = "http://localhost:59928/";
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<FriendsViewModel>();
            SimpleIoc.Default.Register<MapViewModel>();
            SimpleIoc.Default.Register<ILoginService>(() =>
            {
                return new LoginService(baseUrl);
            });

            SimpleIoc.Default.Register<ISettingsService>(() =>
            {
                return new SettingsService();
            });
            SimpleIoc.Default.Register<IFriendService>(() =>
            {
                return new FriendService(SimpleIoc.Default.GetInstance<ISettingsService>(), baseUrl);
            });
        }

        public LoginViewModel Login
        {
            get { return SimpleIoc.Default.GetInstance<LoginViewModel>(); }
        }

        public FriendsViewModel Friends
        {
            get { return SimpleIoc.Default.GetInstance<FriendsViewModel>(); }
        }

        public MapViewModel MapFriend
        {
            get { return SimpleIoc.Default.GetInstance<MapViewModel>(); }
        }
    }
}
