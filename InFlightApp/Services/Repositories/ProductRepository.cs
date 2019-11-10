using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;

namespace InFlightApp.Services.Repositories{
    public class ProductRepository : IProductRepository{
        public string[] GetCategories(){
            return Enum.GetNames(typeof(ProductType));
        }
    }
}
