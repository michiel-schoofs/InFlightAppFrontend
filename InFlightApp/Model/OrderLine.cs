using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model {
    public class OrderLine : INotifyPropertyChanged {
        public Order Order { get; set; }
        public Product Product { get; set; }

        private int _amount;
        public int Amount { 
            get { return _amount; } 
            set { 
                _amount = value;
                RaisePropertyChanged();
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
