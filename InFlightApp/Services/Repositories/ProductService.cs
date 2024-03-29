﻿using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using static System.Net.Mime.MediaTypeNames;

namespace InFlightApp.Services.Repositories{
    public class ProductService : IProductService{
        private readonly static Dictionary<int, string> _images = new Dictionary<int, string>();
        private static StorageFolder sf = ApplicationData.Current.LocalCacheFolder;


        public string[] GetCategories(){
            return Enum.GetNames(typeof(ProductType));
        }

        public async Task<Product[]> GetProducts(ProductType pt){
            string url = $"{ApiConnection.URL}/Products/categories/{Enum.GetName(typeof(ProductType), pt)}";
            string s = ApiConnection.Client.GetStringAsync(url).Result;
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
            if (_images.ContainsKey(productID))
                return _images.GetValueOrDefault(productID);

            string url = $"{ApiConnection.URL}/Products/{productID}/image";
            string s = ApiConnection.Client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);

            if (obj != null && obj.Value<int>("id")>=0) {
                string name = $"prod-{productID}.{obj.Value<string>("extension").ToLower()}";
                byte[] bytes = Convert.FromBase64String(obj.Value<string>("data"));

                try{
                    StorageFile fi = await sf.GetFileAsync(name);
                    await fi.DeleteAsync();
                }catch (Exception) { }

                StorageFile f = await sf.CreateFileAsync(name);

                Stream stream = f.OpenStreamForWriteAsync().Result;
                stream.Write(bytes, 0, bytes.Length);

                _images.Add(productID, f.Path);
                return f.Path;
            }

            return "/Assets/img/products/default.jpg";
        }

        public bool AddToStock(int productID, int restock){
            if (restock <= 0)
                return false;

            var content = new StringContent("",Encoding.UTF8, "application/json");
            string url = $"{ApiConnection.URL}/Products/{productID}/restock/{restock}";

            HttpResponseMessage message = ApiConnection.Client.PutAsync(url, content).Result;

            if (message.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }
    }
}
