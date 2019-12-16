using InFlightAppBACKEND.Models.Domain;

namespace InFlightAppBACKEND.Models.DTO{
    //Not intended to be used to register
    public class PassengerDTO:PersonDTO{

        public SeatDTO Seat { get; set; }
        public int TravelGroupId { get; set; }

        public PassengerDTO(Passenger p):base(p) {
            if(p.Seat != null)
                Seat = new SeatDTO(p.Seat);
            if (p.TravelGroup != null)
                TravelGroupId = p.TravelGroup.TravelGroupId;
        }
    }
}
