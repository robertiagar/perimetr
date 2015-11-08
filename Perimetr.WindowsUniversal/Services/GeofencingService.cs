using GalaSoft.MvvmLight.Messaging;
using Perimetr.WindowsUniversal.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace Perimetr.WindowsUniversal.Services
{
    public class GeofencingService
    {
        private IList<Geofence> geofences = new List<Geofence>();
        private TypedEventHandler<GeofenceMonitor, object> stateChange;
        private TypedEventHandler<GeofenceMonitor, object> statusChanged;
        private CoreDispatcher dispatcher;

        public GeofencingService(TypedEventHandler<GeofenceMonitor, object> stateChange, TypedEventHandler<GeofenceMonitor, object> statusChanged, CoreDispatcher dispatcher)
        {
            Messenger.Default.Register<SetupGeofencingMessage>(this, async message => await SetupGeofencesAsync(message));
            this.stateChange = stateChange;
            this.statusChanged = statusChanged;
            this.dispatcher = dispatcher;
        }

        public async Task SetupGeofencesAsync(SetupGeofencingMessage message)
        {
            foreach (var mapFriendViewModel in message.MapFriendViewModel.Where(f => f.Location != null))
            {
                var geofence = new Geofence(mapFriendViewModel.Id, new Geocircle(mapFriendViewModel.Location.Position, 2000), MonitoredGeofenceStates.Entered, false);
                if (geofences.SingleOrDefault(g => g.Id == geofence.Id) == null)
                    geofences.Add(geofence);
            }

            var settings = ApplicationData.Current.LocalSettings;
            var jsonsString = settings.Values["BackgroundGeofenceEventCollection"];

            await RegisterBackgroundTask();
        }

        async private Task RegisterBackgroundTask()
        {
            // Get permission for a background task from the user. If the user has already answered once,
            // this does nothing and the user must manually update their preference via PC Settings.
            BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            // Regardless of the answer, register the background task. Note that the user can use
            // the Settings app to prevent your app from running background tasks.
            // Create a new background task builder
            BackgroundTaskBuilder geofenceTaskBuilder = new BackgroundTaskBuilder();

            geofenceTaskBuilder.Name = "Geofence";
            geofenceTaskBuilder.TaskEntryPoint = "Perimetr.BackgroundTasks.GeofenceBackgroundTask";

            // Create a new location trigger
            var trigger = new LocationTrigger(LocationTriggerType.Geofence);

            // Associate the locationi trigger with the background task builder
            geofenceTaskBuilder.SetTrigger(trigger);

            // If it is important that there is user presence and/or
            // internet connection when OnCompleted is called
            // the following could be called before calling Register()
            // SystemCondition condition = new SystemCondition(SystemConditionType.UserPresent | SystemConditionType.InternetAvailable);
            // geofenceTaskBuilder.AddCondition(condition);

            // Register the background task
            var geofenceTask = geofenceTaskBuilder.Register();

            // Associate an event handler with the new background task
            geofenceTask.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);

            switch (backgroundAccessStatus)
            {
                case BackgroundAccessStatus.Unspecified:
                case BackgroundAccessStatus.Denied:
                    break;

            }

            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    geofences = GeofenceMonitor.Current.Geofences;

                    // register for state change events
                    GeofenceMonitor.Current.GeofenceStateChanged += stateChange;
                    GeofenceMonitor.Current.StatusChanged += statusChanged;
                    break;

                case GeolocationAccessStatus.Denied:
                    break;

                case GeolocationAccessStatus.Unspecified:
                    break;
            }
        }

        private async void OnCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            if (sender != null)
            {
                // Update the UI with progress reported by the background task
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try
                    {
                        // If the background task threw an exception, display the exception in
                        // the error text box.
                        args.CheckResult();

                        // Update the UI with the completion status of the background task
                        // The Run method of the background task sets the LocalSettings. 
                        var settings = ApplicationData.Current.LocalSettings;

                        // get status
                        if (settings.Values.ContainsKey("Status"))
                        {
                            //rootPage.NotifyUser(settings.Values["Status"].ToString(), NotifyType.StatusMessage);
                        }

                        // do your apps work here
                    }
                    catch (Exception ex)
                    {
                        // The background task had an error
                       // rootPage.NotifyUser(ex.ToString(), NotifyType.ErrorMessage);
                    }
                });
            }
        }

        public void Unsubscribe()
        {
            GeofenceMonitor.Current.GeofenceStateChanged -= stateChange;
            GeofenceMonitor.Current.StatusChanged -= statusChanged;
        }
    }
}
