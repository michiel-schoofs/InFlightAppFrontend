using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model{
    public class ProductViewModel{
        private readonly IProductRepository _prodRepo;
        public string[] Categories { get; set; }
        public RelayCommand SelectCategory { get; set; }

        public ObservableCollection<Product> Products { get; set; }

        public ProductViewModel(){
            try{
                _prodRepo = ServiceLocator.Current.GetService<IProductRepository>(true);
                Categories = _prodRepo.GetCategories();
                Products = new ObservableCollection<Product>();
            }catch (Exception e){
                //Replace with logging later on
                Console.WriteLine(e);
            }

            SetupCommands();
        }

        private void SetupCommands() {
            SelectCategory = new RelayCommand((object o) => { ChangeSelectedCategory((string)o); });
        }

        public void ChangeSelectedCategory(string o) {
            ProductType pt;
            
            if (o == null)
                pt = (ProductType)Enum.Parse(typeof(ProductType), Categories.First());
            else
                pt = (ProductType)Enum.Parse(typeof(ProductType), o);

            Array.ForEach(Products.ToArray(), (Product p) => Products.Remove(p));
            Array.ForEach(_prodRepo.GetProducts(pt).Result,(Product p) => Products.Add(p));
        }
    }
}
