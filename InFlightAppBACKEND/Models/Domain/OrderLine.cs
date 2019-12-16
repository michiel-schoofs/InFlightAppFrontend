using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public class OrderLine
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }

        protected OrderLine(){}

        public OrderLine(Order order, Product product, int amount){
            OrderId = order.OrderId;
            ProductId = product.ProductId;
            Order = order;
            Product = product;
            Amount = amount;
        }
    }
}
