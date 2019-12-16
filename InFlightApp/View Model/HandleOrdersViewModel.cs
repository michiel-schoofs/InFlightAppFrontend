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

        public static ObservableCollection<OrderLine> OrderLines { get;set; }


        public delegate Task CartChangedDelegate();
        public static event CartChangedDelegate CartChanged;

        public RelayCommand ConfirmOrder { get; set; }
        
        public RelayCommand ConfirmApproveOrder { get; set; }
        public RelayCommand ConfirmDenyOrder { get; set; }

        public RelayCommand DeleteProductFromCartCom { get; set; }
        public RelayCommand AddOrderToCart { get; set; }

        public RelayCommand ApproveOrder { get; set; }
        public RelayCommand DenyOrder { get; set; }

        public RelayCommand ClearCart { get; set; }

        public HandleOrdersViewModel() {
            try {
                Orders = new ObservableCollection<Order>();
                OrderLines = new ObservableCollection<OrderLine>(_handleOrdersService.GetCartLines());
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

        public void DeleteProductFromCart(Product prod) {
            _handleOrdersService.RemoveProductFromOrder(prod);
        }

        public int GetAmountOfProductsInCar() {
            return _handleOrdersService.GetAmountOfProductsInCart();
        }

        public void UpdateOrders() {
            var orders = _handleOrdersService.GetAllUnprocessed();
            Orders.Clear();
            foreach (var item in orders) {
                Orders.Add(item);
            }
        }

        private void SendOrder() {
            _handleOrdersService.SendOrder();
        }


        private void ClearCartRepo() {
            _handleOrdersService.ClearCart();
        }

        public void FillList() {
            OrderLines = new ObservableCollection<OrderLine>(_handleOrdersService.GetCartLines());
        }

        private void MakeUserCommands() {
            AddOrderToCart = new RelayCommand((object o) =>{
                object[] obj = (object[])o;
                Product p = ((Product)(obj[0]));
                int amount = int.Parse((string)obj[1]);

                _handleOrdersService.PlaceOrder(p, amount);
                CartChanged.Invoke();
            });

            DeleteProductFromCartCom = new RelayCommand((object o) => {
                Product p = (Product)o;
                DeleteProductFromCart(p);
                OrderLines.Remove(OrderLines.FirstOrDefault(ol => ol.Product.ProductID == p.ProductID));
                CartChanged.Invoke();
            });

            ClearCart = new RelayCommand((object o) => {
                ClearCartRepo();
                OrderLines.Clear();
                CartChanged.Invoke();
            });

            ConfirmOrder = new RelayCommand((object o) => {
                SendOrder();
                ClearCartRepo();
                OrderLines.Clear();
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
