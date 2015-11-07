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
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ILoginService>(() =>
            {
                return new LoginService("http://localhost:59928/");
            });
        }

        public LoginViewModel Login
        {
            get { return SimpleIoc.Default.GetInstance<LoginViewModel>(); }
        }
    }
}
