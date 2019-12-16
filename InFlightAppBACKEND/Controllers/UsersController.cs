using System;
using InFlightAppBACKEND.Models.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using InFlightAppBACKEND.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using InFlightAppBACKEND.Models.Domain;

namespace InFlightAppBACKEND.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPassengerRepository _passengerRepo;
        private readonly IFlightRepository _flightRepo;
        private readonly IConfiguration _config;

        public UsersController( SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager,
                                IPassengerRepository passengerRepo, IConfiguration config,IFlightRepository flightRepo) {
            _signInManager = signInManager;
            _userManager = userManager;
            _passengerRepo = passengerRepo;
            _flightRepo = flightRepo;
            _config = config;
        }

        // Create the token
        private async Task<String> GetToken(IdentityUser user) { 
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName) 
            };


            claims.AddRange(await _userManager.GetClaimsAsync(user));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                null, null, claims.ToArray(),
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize(Policy = "Passenger")]
        [Route("current")]
        [HttpGet]
        public ActionResult<PersonDTO> GetLoggedInUser(){
            string username = User.Identity.Name;
            Console.WriteLine(username);
            User user = _flightRepo.GetByUsername(username);

            if (user == null)
                return NotFound("We couldn't find the user you're looking for");

            return new PersonDTO(user);
        }

        [Authorize(Policy = "Passenger")]
        [Route("current/image")]
        [HttpGet]
        public ActionResult<Image> GetImage(){
            string username = User.Identity.Name;
            User user = _flightRepo.GetByUsername(username);

            if (user.ProfilePicture == null)
                return BadRequest("This user doesn't have a profile picture");

            return user.ProfilePicture;
        }

        [Route("{id}/image")]
        [HttpGet]
        public ActionResult<Image> GetImageForUser(int id) {
            Passenger user = _passengerRepo.GetById(id);

            if (user.ProfilePicture == null)
                return BadRequest("This user doesn't have a profile picture");

            return user.ProfilePicture;
        }

        [Authorize(Policy = "Passenger")]
        [Route("current/image/exist")]
        [HttpGet]
        public ActionResult<bool> ImageExist() {
            string username = User.Identity.Name;
            User user = _flightRepo.GetByUsername(username);
            return user.ProfilePicture != null;
        }


        [Authorize(Policy = "Passenger")]
        [Route("current/travelgroup/exist")]
        [HttpGet]
        public ActionResult<bool> HasTravelgroup() {
            string username = User.Identity.Name;
            User user = _flightRepo.GetByUsername(username);
 
            if (user == null || !(user is Passenger) || ((Passenger)user).TravelGroup == null)
                return false;

            return true;
        }

        [Route("passengers/login/{seatnumber}")]
        [HttpPost]
        public async Task<ActionResult<String>> LoginAsPassenger(int seatnumber)
        {
            var pas = _passengerRepo.GetBySeatNumber(seatnumber);

            if (pas == null)
                return NotFound("There's no user associated with this seat or the seat doesn't exist");

            var user = await GetUser(pas.Username);

            if (user != null)
            {
                if (await CheckPassword(user, pas.Password))
                {

                    if (!(await UserHasClaim(user, "Passenger")))
                        return StatusCode(403, "User isn't a passenger");

                    string token = await GetToken(user);
                    //returns only the token
                    return Created("", token);
                }
            }

            return BadRequest("Something went wrong");
        }

        // [Authorize(Policy = "Crew")]
        [Route("passengers")]
        [HttpGet]
        public ActionResult<IEnumerable<PassengerDTO>> GetAllPassengers() {
            return _passengerRepo.GetAll().Select(p => new PassengerDTO(p)).ToList();
        }

        //[Authorize(Policy = "Crew")]
        [Route("passengers/{id}")]
        [HttpGet]
        public ActionResult<PassengerDTO> GetPassengerByID(int id) {
            Passenger p = _passengerRepo.GetById(id);

            if (p == null)
                return BadRequest("The specified passenger doesn't exist");

            return new PassengerDTO(p);
        }

        [Authorize(Policy = "Passenger")]
        [Route("passengers/{id}/seat")]
        [HttpGet]
        public ActionResult<SeatDTO> GetPassengerSeat(int id) {
            Passenger p = _passengerRepo.GetById(id);

            if (p == null)
                return BadRequest("The specified passenger doesn't exist");
            
            if(p.Seat ==null)
                return BadRequest("The specified passenger doesn't have a seat associated with his account");

            return new SeatDTO(p.Seat);
        }

        // [Authorize(Policy = "Crew")]
        [Route("passengers/{id}/seat/change/{seatNr}")]
        [HttpPut]
        public async Task<ActionResult> ChangeSeatNr(int id,int seatNr) {
            Seat s = _flightRepo.GetSeatById(seatNr);
            Passenger pas = _passengerRepo.GetById(id);

            if (s == null)
                return NotFound("We couldn't find the seat you specified");

            
            Passenger pas2 = _passengerRepo.GetBySeatNumber(seatNr);
            int oldSeatnr = _passengerRepo.GetSeatNumberFromPassenger(pas.UserId);

            if (oldSeatnr != seatNr && pas2 != null)
            {
                string prevUsername = pas.Username;
                string prevPassword = pas.Password;

                pas2 = _passengerRepo.GetById(pas2.UserId);

                if (!(await ChangeUserToSeat(pas2, oldSeatnr, null,null)))
                    return BadRequest("There's already someone on this seat");

                bool b = await ChangeUserToSeat(pas, seatNr,prevUsername,prevPassword);

                if (b)
                    return Ok();
                else
                    return BadRequest("Something went wrong");
            }
            else {
                bool b = await ChangeUserToSeat(pas, seatNr,null,null);

                if (b)
                    return Ok();
                else
                    return BadRequest("Something went wrong");
            }
        }

        [Authorize(Policy = "Crew")]
        [Route("checkToken")]
        [HttpGet]
        public ActionResult<IEnumerable<PassengerDTO>> CheckToken() {
            return Ok();
        }

            private async Task<bool> ChangeUserToSeat(Passenger pas, int seatnr,string username,string password) {
            IdentityUser iu;

            if (username==null)
                iu = await _userManager.FindByNameAsync(pas.Username);
            else
                iu = await _userManager.FindByNameAsync(username);

            Seat s = _flightRepo.GetSeatById(seatnr);

            if (iu != null){
                string oldPass = password;

                if (oldPass == null)
                    oldPass = pas.Password;

                pas.Seat = s;

                await _userManager.ChangePasswordAsync(iu, oldPass, pas.Password);

                iu.UserName = pas.Username;
                iu.NormalizedUserName = iu.UserName.ToUpper();
                iu.Email = $"{iu.UserName}@testmail.be";
                iu.NormalizedEmail = iu.Email.ToUpper();

                _passengerRepo.SaveChanges();
                return true;
            }

            return false;
        }

        [Route("crewmember/login")]
        [HttpPost]
        public async Task<ActionResult<String>> LoginAsCrew(LoginDTO model){
            var user = await GetUser(model.Username);
            if (user != null){
                if (await CheckPassword(user, model.Password)) {

                    if (!(await UserHasClaim(user,"Crew")))
                        return StatusCode(403,"User isn't a crewmember");

                    string token = await GetToken(user);
                    //returns only the token
                    return Created("", token); 
                }
            }

            return BadRequest("We couldn't find this user or the password is incorrect");
        }

        private async Task<IdentityUser> GetUser(string username) {
            return await _userManager.FindByNameAsync(username);
        }

        private async Task<bool> UserHasClaim(IdentityUser user,String role) {
            var claims = await _userManager.GetClaimsAsync(user);
            var cl = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value.Equals(role));

            return cl != null;
        }

        private async Task<bool> CheckPassword(IdentityUser iu, string password) {
            var res = await _signInManager.CheckPasswordSignInAsync(iu, password, false);
            return res.Succeeded;
        }
    }
}
