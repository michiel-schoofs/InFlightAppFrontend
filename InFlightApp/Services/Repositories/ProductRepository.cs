using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using static System.Net.Mime.MediaTypeNames;

namespace InFlightApp.Services.Repositories{
    public class ProductRepository : IProductRepository{
        private HttpClient client;
        private readonly static Dictionary<int, string> _images = new Dictionary<int, string>();

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

        public async Task<string> GetImage(int productID){
            string url = $"{ApiConnection.URL}/Products/{productID}/image";
            string s = client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);

            if (obj != null && obj.Value<int>("id")>=0) {
                string name = $"prod-{productID}.{obj.Value<string>("extension").ToLower()}";
                byte[] bytes = Convert.FromBase64String(obj.Value<string>("data"));

                StorageFolder sf = ApplicationData.Current.LocalCacheFolder;

                try{
                    StorageFile fi = await sf.GetFileAsync(name);
                    await fi.DeleteAsync();
                }catch (Exception) { }

                StorageFile f = await sf.CreateFileAsync(name);

                Stream stream = f.OpenStreamForWriteAsync().Result;
                stream.Write(bytes, 0, bytes.Length);

                return f.Path;
            }

            return "/Assets/img/products/default.jpg";
        }
    }
}
