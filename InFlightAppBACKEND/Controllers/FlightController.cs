using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using InFlightAppBACKEND.Data.Repositories;
using InFlightAppBACKEND.Models.DTO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using InFlightAppBACKEND.Models.Domain;

namespace InFlightAppBACKEND.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase{
        private readonly IFlightRepository _flightRepo; 

        public FlightController(IFlightRepository flightRepo){
            _flightRepo = flightRepo;
        }

        [HttpGet("seats/{id}/user")]
        public PassengerDTO GetPassengerFromSeat(int id) {
            Seat s = _flightRepo.GetSeatById(id);
            return new PassengerDTO(s.Passenger);
        }

        [HttpGet("seats/{id}/user/exist")]
        public bool SeatHasUser(int id) {
            Seat s = _flightRepo.GetSeatById(id);
            return s.Passenger != null;
        }

        [HttpGet("seats")]
        public IEnumerable<SeatDTO> GetSeats() {
            return _flightRepo.GetAllSeats().Select(s=>new SeatDTO(s));
        }

        [HttpGet("info")]
        public FlightInfoDTO GetInfo() {
            return new FlightInfoDTO(_flightRepo.GetAll().First());
        }
    }
}