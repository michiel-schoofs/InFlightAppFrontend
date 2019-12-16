using InFlightAppBACKEND.Models.Domain;
using System;

namespace InFlightAppBACKEND.Models.DTO{
    public class SeatDTO{
        public int SeatID{ get; set; }
        public string SeatCode{ get; set; }
        public string Type { get; set; }

        public SeatDTO(Seat s){
            SeatID = s.SeatId;
            SeatCode = s.SeatCode;
            Type = Enum.GetName(typeof(SeatType), s.Type);
        }
    }
}
