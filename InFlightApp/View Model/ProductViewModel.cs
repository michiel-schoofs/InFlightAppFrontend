using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace InFlightApp.View_Model{
    public class ProductViewModel{
        public static IProductRepository _prodRepo;

        private string _nameFilter;

        public string NameFilter {
            get { return _nameFilter; }
            set { 
                _nameFilter = value;
                SortProducts();
            }
        }

        private SortType _sort;

        public SortType Sort {
            get { return _sort; }
            set {
                _sort = value;
                SortProducts();
            }
        }

        private bool available;

        public bool OnlyShowAvailable {
            get { return available; }
            set { 
                available = value;
                SortProducts();
            }
        }

        public string[] Categories { get; set; }

        public RelayCommand SelectCategory { get; set; }
        public RelayCommand KeyUpTextBox { get; set; }

        public ObservableCollection<string> SortModes { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; }

        public delegate void SelectionChangedDel();
        public event SelectionChangedDel SelectionChanged;

        public ProductViewModel(){
            try{
                _prodRepo = ServiceLocator.Current.GetService<IProductRepository>(true);
                Categories = _prodRepo.GetCategories();

                Products = new ObservableCollection<Product>();
                FilteredProducts = new ObservableCollection<Product>();

                SortModes = new ObservableCollection<string>(Enum.GetNames(typeof(SortType)));

                ResetFilters();
            }
            catch (Exception e){
                //Replace with logging later on
                Console.WriteLine(e);
            }

            SetupCommands();
        }

        private void ResetFilters() {
            OnlyShowAvailable = false;
            NameFilter = ""; 
            Sort = SortType.None;
        }

        private void SetupCommands() {
            SelectCategory = new RelayCommand((object o) => { ChangeSelectedCategory((string)o); });
            KeyUpTextBox = new RelayCommand((object o) =>{
                NameFilter = (string)o;
            });
        }

        public void SortProducts() {
            //Replace with filtered list
            var prods = Products.Where(p =>
            {
                return (String.IsNullOrEmpty(NameFilter)?true:p.Name.ToLower().StartsWith(NameFilter.ToLower()))
                       && (OnlyShowAvailable ? p.Amount > 0 : true);
            }).ToArray();

            switch (Sort) {
                case SortType.Alpha:
                    prods = prods.OrderBy(p => p.Name).ToArray();
                    break;
                case SortType.PriceAsc:
                    prods = prods.OrderBy(p => p.UnitPrice).ToArray();
                    break;
                case SortType.PriceDesc:
                    prods = prods.OrderBy(p => p.UnitPrice).Reverse().ToArray();
                    break;
                default:
                    break;
            }

            Array.ForEach(FilteredProducts.ToArray(), (Product p) => FilteredProducts.Remove(p));
            Array.ForEach(prods, p => FilteredProducts.Add(p));
            
        }

        public async void ChangeSelectedCategory(string o) {
            ProductType pt;

            if (o == null)
                pt = (ProductType)Enum.Parse(typeof(ProductType), Categories.First());
            else
                pt = (ProductType)Enum.Parse(typeof(ProductType), o);

            Array.ForEach(Products.ToArray(), (Product p) => Products.Remove(p));
            Array.ForEach(_prodRepo.GetProducts(pt).Result,(Product p) => {
                Products.Add(p);
            });

            ResetFilters();
            SelectionChanged?.Invoke();
        }

        public async void GetImages(){
            try{
                foreach (Product p in Products) {
                    string path = await _prodRepo.GetImage(p.ProductID);
                    p.ImageFile = path;
                }
            }catch (Exception) { }
        }

    }
}
