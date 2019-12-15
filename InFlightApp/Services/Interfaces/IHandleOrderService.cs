using InFlightApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Interfaces {
    interface IHandleOrderService {
        OrderLine[] GetCartLines();
        void ClearCart();
        IEnumerable<Order> GetAllUnprocessed();
        int GetAmountOfProductsInCart();
        void ApproveOrder(int id);
        void DenyOrder(int id);
        void PlaceOrder(Product prod, int amount);
        void RemoveProductFromOrder(Product prod);
        int GetAmountInCart(Product prod);
        void SendOrder();
    }
}
