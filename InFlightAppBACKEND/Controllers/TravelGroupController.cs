using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InFlightAppBACKEND.Data.Repositories;
using InFlightAppBACKEND.Models.Domain;
using InFlightAppBACKEND.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InFlightAppBACKEND.Controllers
{
    [Authorize(Policy = "Passenger")]
    [Route("api/[controller]")]
    public class TravelGroupController : Controller
    {
        private readonly ITravelGroupRepository _travelGroupRepository;
        private readonly IFlightRepository _flightRepo;

        public TravelGroupController(ITravelGroupRepository repos, IFlightRepository flightRepo) {
            _travelGroupRepository = repos;
            _flightRepo = flightRepo;
        }


        [Route("exist")]
        [HttpGet]
        public ActionResult<bool> UserInTravelGroup() {
            Passenger pas = GetPassenger();
            return pas.TravelGroup != null;
        }

        [Route("messages")]
        [HttpGet]
        public ActionResult<IEnumerable<MessageDTO>> GetMessages() {
            Passenger pas = GetPassenger();

            if (pas.TravelGroup == null)
                return BadRequest("This user is not in a travelgroup");

            return _travelGroupRepository.GetMessagesFromTravelGroup(pas.TravelGroup.TravelGroupId).Select(message => new MessageDTO(message)).ToList();
        }

        [Route("messages")]
        [HttpPost]
        public ActionResult<MessageDTO> PostMessage(string content) {
            Passenger pas = GetPassenger();
            Message message = _travelGroupRepository.AddMessage(pas.TravelGroup.TravelGroupId, pas, content);
            _travelGroupRepository.SaveChanges();
            return new MessageDTO(message);
        }


        private Passenger GetPassenger() {
            User user = _flightRepo.GetByUsername(User.Identity.Name);

            if (user == null)
                throw new ArgumentNullException("We couldn't find the user you're looking for");

            if (!(user is Passenger))
                throw new ArgumentException("This user isn't a passenger");

            return (Passenger)user;
        }
    }
}