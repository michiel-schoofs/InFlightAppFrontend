using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Configuration
{
 public class Startup{
            static private readonly ServiceCollection _serviceCollection = new ServiceCollection();

            static public async Task ConfigureAsync(){
                ServiceLocator.Configure(_serviceCollection);
            }
    }
}
