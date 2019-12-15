using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Repositories {
    public class HandleOrderService : IHandleOrderService {
        private static readonly IDictionary<int, OrderLine> _cart = new Dictionary<int, OrderLine>();
        private static Order order;
        private HttpClient client;

        public HandleOrderService() {
            client = ApiConnection.Client;
        }

        public OrderLine[] GetCartLines() {
            return _cart.Values.ToArray();
        }

        public void PlaceOrder(Product prod, int amount) {

            if (!_cart.ContainsKey(prod.ProductID))
                _cart.Add(prod.ProductID, new OrderLine() { Amount = amount, Product = prod});
            else
                _cart[prod.ProductID].Amount += amount;
        }

        public void RemoveProductFromOrder(Product prod) {
            if(_cart.ContainsKey(prod.ProductID))
                _cart.Remove(prod.ProductID);
        }

        public int GetAmountInCart(Product prod) {
            if (_cart.ContainsKey(prod.ProductID))
                return _cart[prod.ProductID].Amount;

            return -1;
        }

        public void ApproveOrder(int id) {
            string url = $"{ApiConnection.URL}/Order/crew/approve/{id}";
            client.PostAsync(url, null).Wait();
        }

        public void DenyOrder(int id) {
            string url = $"{ApiConnection.URL}/Order/crew/deny/{id}";
            client.PostAsync(url, null).Wait();
        }

        public IEnumerable<Order> GetAllUnprocessed() {
            string url = $"{ApiConnection.URL}/Order/crew/orders";
            string s = client.GetStringAsync(url).Result;

            JArray ar = JArray.Parse(s);

            return ar.Select(o => {
                var token = o.Value<JToken>("passenger");
                JObject persoonObj = JObject.Parse(token.ToString());

                token = persoonObj.Value<JToken>("seat");
                JObject seatObj = JObject.Parse(token.ToString());

                Passenger p = new Passenger {
                    Id = persoonObj.Value<int>("id"),
                    FirstName = persoonObj.Value<string>("firstName"),
                    LastName = persoonObj.Value<string>("lastName"),
                    TravelGroupId = persoonObj.Value<int>("travelGroupId"),
                    Seat = new Seat { 
                        SeatId = seatObj.Value<int>("seatID"),
                        SeatCode = seatObj.Value<string>("seatCode")
                    }
                };

                token = o.Value<JToken>("orderLines");
                JArray orderlinesObj = JArray.Parse(token.ToString());
                var orderlines = orderlinesObj.Select(ol => {
                    token = ol.Value<JToken>("product");
                    JObject productObj = JObject.Parse(token.ToString());
                    Product product = new Product {
                        ProductID = productObj.Value<int>("productID"),
                        Name = productObj.Value<string>("name"),
                        UnitPrice = productObj.Value<decimal>("productPrice"),
                    };
                    return new OrderLine {
                        Amount = ol.Value<int>("amount"),
                        Product = product
                    };
                });

                Order ord =  new Order() {
                    OrderId = o.Value<int>("orderID"),
                    OrderDate = DateTime.Parse(o.Value<string>("orderDate")),
                    IsDone = o.Value<bool>("isDone"),
                    Passenger = p,
                    OrderLines = orderlines
                };
                foreach (var orderl in ord.OrderLines) {
                    orderl.Order = ord;
                }
                return ord;
            }).ToArray();
        }

        public int GetAmountOfProductsInCart() {
            return _cart.Keys.Count;
        }

        public void ClearCart(){
            _cart.Clear();
        }

        public void SendOrder(){
            string url = $"{ApiConnection.URL}/Order/";

            JArray ja = ParseToOrderLine();
            var data = new StringContent(ja.ToString(), Encoding.UTF8, "application/json");

            try{
                client.PostAsync(url, data).Wait();
            }catch (Exception e) { }
        }

        private JArray ParseToOrderLine() {
            JArray ja = new JArray();

            for (int i = 0;i < _cart.Count();i++) {
                JObject parsed = new JObject();
                parsed.Add("productID",_cart.Keys.ElementAt(i));
                parsed.Add("amount", _cart.Values.ElementAt(i).Amount);
                ja.Add(parsed);
            }

            return ja;
        }

    }
}
