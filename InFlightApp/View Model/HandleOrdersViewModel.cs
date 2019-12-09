using InFlightApp.Configuration;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model {
    public class HandleOrdersViewModel {
        private readonly IHandleOrderService _handleOrdersService;

        public HandleOrdersViewModel() {
            try {
                _handleOrdersService = ServiceLocator.Current.GetService<IHandleOrderService>(true);
            } catch (Exception e) {
                //Replace with logging later on
                Console.WriteLine(e);
            }
        }


    }
}
