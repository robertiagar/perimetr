using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Perimetr.WindowsUniversal.Messages;
using Perimetr.WindowsUniversal.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Perimetr.WindowsUniversal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Send(new GetMapPinsMessage());

            try
            {
                SimpleIoc.Default.Register<GeofencingService>(() =>
                {
                    return new GeofencingService(StateChanged, StatusChanged, Dispatcher);
                }, true);
            }
            catch
            {
                //registered
            }
        }

        private void StatusChanged(GeofenceMonitor sender, object args)
        {
            ;//upsie
        }

        private void StateChanged(GeofenceMonitor sender, object args)
        {
            ;//upsie
        }
    }
}
