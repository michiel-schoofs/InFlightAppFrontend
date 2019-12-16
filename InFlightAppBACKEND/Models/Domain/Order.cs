using InFlightAppBACKEND.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public class Order
    {
        public int OrderId { get; set; }
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }
        public DateTime OrderDate { get; set; }
        public Boolean IsDone { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }

        protected Order()
        {
            OrderLines = new List<OrderLine>();
        }

        public Order(Passenger pas)
        {
            Passenger = pas;
            OrderDate = DateTime.Now;
            OrderLines = new List<OrderLine>();
        }

        public Order(bool isdone,DateTime od,Passenger pas):this(){
            IsDone = isdone;
            OrderDate = od;
            Passenger = pas;
        }

        public void AddOrderLine(Product product, int amount){
            if (amount <= 0)
                throw new ArgumentException("Please provide a valid amount to order");

            if (product.Amount < amount)
                throw new ArgumentException("There's not enough to order this much");

            product.Amount -= amount;
            OrderLine ol = OrderLines.FirstOrDefault(o=> o.Product.ProductId == product.ProductId);

            if (ol == null){
                ol = new OrderLine(this, product, amount);
                OrderLines.Add(ol);
            }else{
                ol.Amount += amount;
            }
        }

        public void RemoveOrderLine(Product product)
        {
            OrderLine orderLine = OrderLines.SingleOrDefault(ol => ol.ProductId == product.ProductId);
            OrderLines.Remove(orderLine);
        }

    }
}
