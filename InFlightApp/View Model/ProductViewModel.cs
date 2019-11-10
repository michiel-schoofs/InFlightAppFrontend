using InFlightApp.Configuration;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model{
    public class ProductViewModel{
        private readonly IProductRepository _prodRepo;
        public string[] Categories { get; set; }
        public RelayCommand SelectCategory { get; set; }

        public ProductViewModel(){
            try{
                _prodRepo = ServiceLocator.Current.GetService<IProductRepository>(true);
                Categories = _prodRepo.GetCategories();
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
            Console.WriteLine(o);
        }
    }
}
