using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model {
    public class HandleOrdersViewModel {
        private readonly IHandleOrderService _handleOrdersService;
        public ObservableCollection<Order> Orders { get; private set; }

        public HandleOrdersViewModel() {
            try {
                _handleOrdersService = ServiceLocator.Current.GetService<IHandleOrderService>(true);
                updateOrders();
            } catch (Exception e) {
                //Replace with logging later on
                Console.WriteLine(e);
            }
        }

        private void updateOrders() {
            var orders = _handleOrdersService.GetAllUnprocessed();
            Orders = new ObservableCollection<Order>();
            foreach (var item in orders) {
                Orders.Add(item);
            }
        }
    }
}
