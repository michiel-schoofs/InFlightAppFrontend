using InFlightAppBACKEND.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data
{
    public class DBInitializer
    {
        private readonly Random rand = new Random();

        private DBContext _dbContext;
        private UserManager<IdentityUser> _userManager;

        private Passenger[] passengers;
        private CrewMember[] crewMembers;
        private Product[] producten;

        public DBInitializer(DBContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task seedDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                #region Flight
                Origin origin = new Origin() { IATA = "BRU", City = "Brussels", Country = "Belgium" };
                Destination destination = new Destination() { IATA = "SJO", City = "San Jose", Country = "Costa Rica" };
                Flight flight1 = new Flight("BRUSJO-871X", "Boeing 737", DateTime.Now, DateTime.Now.AddHours(8), origin, destination, "Brussels airlines");
                _dbContext.Flights.Add(flight1);
                #endregion

                SeedProductJSON();
                SeedCrewMembers();

                #region Seat
                Seat[] seats = new Seat[] {
                        new Seat(SeatType.BUSINESS,"1A"),new Seat(SeatType.ECONOMY,"1B"),
                        new Seat(SeatType.FIRST_CLASS,"2A"), new Seat(SeatType.BUSINESS,"2B"),
                        new Seat(SeatType.BUSINESS,"3A"), new Seat(SeatType.BUSINESS,"3B"),
                        new Seat(SeatType.BUSINESS,"4A"), new Seat(SeatType.BUSINESS,"4B"),
                        new Seat(SeatType.BUSINESS,"5A"),new Seat(SeatType.BUSINESS,"5B")
                    };

                Array.ForEach(seats, (Seat s) =>{
                    flight1.AddSeat(s);
                });
                #endregion

                SeedPassengers(seats);

                #region Order
                ICollection<Order> orders = new List<Order>();

                //If product already in order, change orderline add product domain
                Array.ForEach(passengers, (Passenger pas) =>
                {
                    Order ord = new Order(pas);

                    Product prod1 = producten[rand.Next(producten.Length)];
                    Product prod2 = producten[rand.Next(producten.Length)];

                    if (prod1.Amount >= 10)
                        ord.AddOrderLine(prod1, rand.Next(1, 10));

                    if (prod2.Amount >= 10)
                        ord.AddOrderLine(prod2, rand.Next(1, 10));

                    orders.Add(ord);
                });

                _dbContext.Orders.AddRange(orders);
                _dbContext.SaveChanges();
                #endregion
            }
        }

        private void SeedCrewMembers() {
            JObject o1 = JObject.Parse(File.ReadAllText(@".\Data\Seeding\users.json"));
            JArray a1 = o1.Value<JArray>("Crewmembers");

            List<CrewMember> cmList = new List<CrewMember>();

            foreach (JToken jt in a1) {
                string firstName = jt.Value<string>("FirstName");
                string lastName = jt.Value<string>("LastName");
                Image pfp = GetImageFromLocalPath(jt.Value<string>("Image"));

                CrewMember cm = new CrewMember(firstName, lastName);
                cm.ProfilePicture = pfp;

                cmList.Add(cm);
                _dbContext.CrewMembers.Add(cm);
                _dbContext.SaveChanges();
            }

            crewMembers = cmList.ToArray();
        }

        private void SeedProductJSON()
        {
            Product product3 = new Product("Marbloro Red 20", "20-pack of Marbloro Red", 7M, ProductType.CIGARETTES, 30);
            Product product5 = new Product("Swatch watch", "The newest Swiss watch on the market!", 149.99M, ProductType.OTHER, 30);

            JObject o1 = JObject.Parse(File.ReadAllText(@".\Data\Seeding\Products.json"));
            JArray a1 = o1.Value<JArray>("Products");

            producten = a1.Select(a =>
            {
                Image img = GetImageFromLocalPath(a.Value<string>("Image"));
                Product prod = null;

                if (a.Value<string>("Amount") != null)
                {
                    prod = new Product(a.Value<string>("Name"), a.Value<string>("Description"), a.Value<decimal>("Price"),
                                    (ProductType)Enum.Parse(typeof(ProductType), a.Value<string>("Type")), a.Value<int>("Amount"));
                }
                else
                {
                    prod = new Product(a.Value<string>("Name"), a.Value<string>("Description"), a.Value<decimal>("Price"),
                        (ProductType)Enum.Parse(typeof(ProductType), a.Value<string>("Type")));
                }

                prod.Image = img;
                return prod;
            }).Append(product3).Append(product5).ToArray();
            _dbContext.AddRange(producten);
        }

        private void SeedPassengers(Seat[] seats) {
            JObject o1 = JObject.Parse(File.ReadAllText(@".\Data\Seeding\users.json"));
            JArray a1 = o1.Value<JArray>("Passengers");

            ICollection<Passenger> passengers = new List<Passenger>();
            TravelGroup tg = new TravelGroup();

            foreach (JToken p in a1) { 
                Passenger pas = new Passenger(p.Value<string>("FirstName")
                    ,p.Value<string>("LastName"));

                string seat = p.Value<string>("SeatNr");
                Seat s = seats.FirstOrDefault(se => se.SeatCode.ToUpper()
                    .Equals(seat.ToUpper()));

                s.Passenger = pas;
                passengers.Add(pas);

                if (p.Value<bool>("inTravelGroup")) {
                    tg.Passengers.Add(pas);
                }

                Image pfp = GetImageFromLocalPath(p.Value<string>("Image"));
                pas.ProfilePicture = pfp;

                _dbContext.Passengers.Add(pas);
                _dbContext.SaveChanges();
            }

            _dbContext.TravelGroups.Add(tg);
            _dbContext.SaveChanges();

            foreach (Passenger pas in tg.Passengers) {
                pas.sendMessage($"Hallo mijn naam is {pas.FirstName}");
            }
            _dbContext.SaveChanges();
            this.passengers = passengers.ToArray();
        }

        public Image GetImageFromLocalPath(string path) {
            byte[] data = File.ReadAllBytes(path);
            string extension = path.Split(".").Last();
            return new Image() { Extension = extension, Data = Convert.ToBase64String(data)};
        }

        public Image GetImage(string url)
        {
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(url);

            string[] ext = url.Split(".");
            string extension = ext.Last().Contains("png") ? "Png" : "Jpeg";

            return new Image() { Data = Convert.ToBase64String(data), Extension = extension };
        }

        public async Task seedIdentityDatabase()
        {
            #region Passenger
            foreach (Passenger p in passengers)
            {
                var identityUser = new IdentityUser()
                {
                    UserName = p.Username,
                    Email = $"{p.Username}@testmail.be",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var res = await _userManager.CreateAsync(identityUser, p.Password);

                if (res.Succeeded)
                {
                    await _userManager.AddClaimAsync(identityUser, new Claim(ClaimTypes.Role, "Passenger"));
                }
            };
            #endregion

            foreach (CrewMember cm in crewMembers)
            {
                var iu = new IdentityUser()
                {
                    UserName = cm.Username,
                    Email = $"{cm.Username}@testmail.be",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var res = await _userManager.CreateAsync(iu, "password-123");

                if (res.Succeeded)
                {
                    await _userManager.AddClaimAsync(iu, new Claim(ClaimTypes.Role, "Crew"));
                    await _userManager.AddClaimAsync(iu, new Claim(ClaimTypes.Role, "Passenger"));
                }
                else
                {
                    Console.Write(res.Errors);
                }
            }

            _dbContext.SaveChanges();
        }

    }
}
