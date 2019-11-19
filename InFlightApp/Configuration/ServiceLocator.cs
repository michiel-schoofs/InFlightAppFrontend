using InFlightApp.Services.Interfaces;
using InFlightApp.Services.Repositories;
using InFlightApp.View_Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace InFlightApp.Configuration
{

    public class ServiceLocator : IDisposable
    {
        static private readonly ConcurrentDictionary<int, ServiceLocator> _serviceLocators = new ConcurrentDictionary<int, ServiceLocator>();

        static private ServiceProvider _rootServiceProvider = null;

        #region Configuration
        static public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUserInterface, UserInterface>()
                             .AddSingleton<LoginViewModel>()
                             .AddSingleton<ProductViewModel>()
                             .AddSingleton<PassengersViewModel>()
                             .AddSingleton<NotificationsViewModel>()
                             .AddSingleton<IProductRepository, ProductRepository>()
                             .AddSingleton<IFlightRepository, FlightRepository>();

            _rootServiceProvider = serviceCollection.BuildServiceProvider();
        }
        #endregion

        static public ServiceLocator Current {
            get {
                int currentViewId = ApplicationView.GetForCurrentView().Id;
                return _serviceLocators.GetOrAdd(currentViewId, key => new ServiceLocator());
            }
        }

        private IServiceScope _serviceScope = null;

        private ServiceLocator()
        {
            _serviceScope = _rootServiceProvider.CreateScope();
        }

        #region Get Service
        public T GetService<T>()
        {
            return GetService<T>(true);
        }

        public T GetService<T>(bool isRequired)
        {
            if (isRequired)
            {
                return _serviceScope.ServiceProvider.GetRequiredService<T>();
            }
            return _serviceScope.ServiceProvider.GetService<T>();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_serviceScope != null)
                {
                    _serviceScope.Dispose();
                }
            }
        }
        static public void DisposeCurrent()
        {
            int currentViewId = ApplicationView.GetForCurrentView().Id;
            if (_serviceLocators.TryRemove(currentViewId, out ServiceLocator current))
            {
                current.Dispose();
            }
        }
        #endregion
    }
}
