using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model {
    public class HandleOrdersViewModel {
        private static IHandleOrderService _handleOrdersService = ServiceLocator.Current.GetService<IHandleOrderService>(true);
        public ObservableCollection<Order> Orders { get; private set; }

        private readonly Subject<Order> _popUpApprove = new Subject<Order>();
        public IObservable<Order> PopUpApprove { get => _popUpApprove; }

        private readonly Subject<Order> _popUpDeny = new Subject<Order>();
        public IObservable<Order> PopUpDeny { get => _popUpDeny; }

        public delegate Task CartChangedDelegate();
        public event CartChangedDelegate CartChanged;

        public RelayCommand ConfirmApproveOrder { get; set; }
        public RelayCommand ConfirmDenyOrder { get; set; }

        public RelayCommand AddOrderToCart { get; set; }

        public RelayCommand ApproveOrder { get; set; }
        public RelayCommand DenyOrder { get; set; }

        public HandleOrdersViewModel() {
            try {
                Orders = new ObservableCollection<Order>();
                MakeCommands();
                MakeUserCommands();
                UpdateOrders();

            } catch (Exception e) {
                //Replace with logging later on
                Console.WriteLine(e);
            }
        }

        public int GetAmountInCart(Product prod) {
            return _handleOrdersService.GetAmountInCart(prod);
        }

        public int GetAmountOfProductsInCar() {
            return _handleOrdersService.GetAmountOfProductsInCart();
        }

        private void UpdateOrders() {
            var orders = _handleOrdersService.GetAllUnprocessed();
            Orders.Clear();
            foreach (var item in orders) {
                Orders.Add(item);
            }
        }

        private void MakeUserCommands() {
            AddOrderToCart = new RelayCommand((object o) =>{
                object[] obj = (object[])o;
                Product p = ((Product)(obj[0]));
                int amount = int.Parse((string)obj[1]);

                _handleOrdersService.PlaceOrder(p, amount);
                CartChanged.Invoke();
            });
        }

        private void MakeCommands() {
            ConfirmApproveOrder = new RelayCommand((object o) => {
                if (o.GetType() == typeof(Order)) {
                    var order = (Order)o;
                    _popUpApprove.OnNext(order);
                }
            });

            ConfirmDenyOrder = new RelayCommand((object o) => {
                if (o.GetType() == typeof(Order)) {
                    var order = (Order)o;
                    _popUpDeny.OnNext(order);
                }
            });

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
        }
    }
}
