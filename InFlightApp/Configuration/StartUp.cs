using InFlightApp.View_Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;

namespace InFlightApp.Configuration
{
    public class Startup
    {
        static private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        static private NotificationsViewModel _notificationsViewModel;

        static public void ConfigureAsync()
        {
            ServiceLocator.Configure(_serviceCollection);
            _notificationsViewModel = ServiceLocator.Current.GetService<NotificationsViewModel>(true);
            ConfigureCortana();
            PollNotifications();
        }

        static private async void ConfigureCortana()
        {
            Uri uriVoiceCommands = new Uri("ms-appx:///Configuration/Cortana/vcd.xml", UriKind.Absolute);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uriVoiceCommands);
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);
        }

        static void PollNotifications()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    _notificationsViewModel.LoadMostRecentNotification();
                    await Task.Delay(5000);
                }
            });
        }
    }
}
