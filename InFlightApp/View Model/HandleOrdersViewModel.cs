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

        public RelayCommand ApproveOrder { get; set; }
        public RelayCommand DenyOrder { get; set; }

        public HandleOrdersViewModel() {
            try {
                Orders = new ObservableCollection<Order>();
                _handleOrdersService = ServiceLocator.Current.GetService<IHandleOrderService>(true);
                UpdateOrders();
                
                ApproveOrder = new RelayCommand((object o) => {
                    if (o.GetType() == typeof(Order)) {
                        var order = (Order)o;
                        _handleOrdersService.ApproveOrder(order.OrderId);
                        UpdateOrders();
                    }
                });
                
                DenyOrder = new RelayCommand((object o) => {
                    if (o.GetType() == typeof(Order)) {
                        var order = (Order)o;
                        _handleOrdersService.DenyOrder(order.OrderId);
                        UpdateOrders();
                    }
                });

            } catch (Exception e) {
                //Replace with logging later on
                Console.WriteLine(e);
            }
        }

        private void UpdateOrders() {
            var orders = _handleOrdersService.GetAllUnprocessed();
            Orders.Clear();
            foreach (var item in orders) {
                Orders.Add(item);
            }
        }
    }
}
