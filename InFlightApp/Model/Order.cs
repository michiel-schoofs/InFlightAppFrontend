using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model {
    public class Order : INotifyPropertyChanged {

        public int OrderId { get; set; }
        public Passenger Passenger { get; set; }
        public DateTime OrderDate { get; set; }
        public Boolean IsDone { get; set; }
        public IEnumerable<OrderLine> OrderLines { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
