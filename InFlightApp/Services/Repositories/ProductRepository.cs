using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InFlightApp.Services.Repositories{
    public class ProductRepository : IProductRepository{
        private HttpClient client;

        public ProductRepository(){
            client = ApiConnection.Client;
        }

        public string[] GetCategories(){
            return Enum.GetNames(typeof(ProductType));
        }

        public async Task<Product[]> GetProducts(ProductType pt){
            string url = $"{ApiConnection.URL}/Products/categories/{Enum.GetName(typeof(ProductType), pt)}";
            string s = client.GetStringAsync(url).Result;
            JArray ar = JArray.Parse(s);

            return ar.Select(p =>{
                return new Product()
                {
                    Name = p.Value<string>("name"),
                    Amount = p.Value<int>("amount"),
                    Description = p.Value<string>("description"),
                    ProductID = p.Value<int>("productID"),
                    Type = (ProductType)Enum.Parse(typeof(ProductType), p.Value<string>("category")),
                    UnitPrice = p.Value<decimal>("productPrice")
                };
            }).ToArray();
        }
    }
}
