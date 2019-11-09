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
 public class Startup{
            static private readonly ServiceCollection _serviceCollection = new ServiceCollection();

            static public void ConfigureAsync(){
                ServiceLocator.Configure(_serviceCollection);
                ConfigureCortana();
            }

            static private async void ConfigureCortana() {
                 Uri uriVoiceCommands = new Uri("ms-appx:///Configuration/Cortana/vcd.xml", UriKind.Absolute);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uriVoiceCommands);
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);
            }
    }
}
