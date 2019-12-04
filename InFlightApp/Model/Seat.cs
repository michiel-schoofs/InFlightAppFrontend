using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model
{
    public class Seat
    {
       /*
        public int Col { get {
                char s = SeatCode[SeatCode.Length - 1];
                return char.ToUpper(s) - 64;
            } 
        }
        public int Row { get {
                string s = SeatCode.Substring(0, SeatCode.Length - 1);
                return int.Parse(s);
            } 
        }*/
        public int SeatId { get; set; }
        public SeatType Type { get; set; }
        public string ImagePath { get {
                switch (Type) {
                    case SeatType.FIRST_CLASS:
                        return "/Assets/img/seats/firstclass.png";
                    case SeatType.BUSINESS:
                        return "/Assets/img/seats/businessclass.png";
                    default:
                        return "/Assets/img/seats/economyclass.png";
                }
            }
        }
        public string SeatCode { get; set; }
    }
}
