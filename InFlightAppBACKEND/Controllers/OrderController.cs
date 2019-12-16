using System;
using System.Collections.Generic;
using System.Linq;
using InFlightAppBACKEND.Data.Repositories;
using InFlightAppBACKEND.Models.DTO;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InFlightAppBACKEND.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly IFlightRepository _flightRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public OrderController(IFlightRepository flightRepo, IOrderRepository orderRepo,
            IProductRepository prodRepo) {
            _flightRepo = flightRepo;
            _orderRepo = orderRepo;
            _productRepo = prodRepo;
        }

        [Route("history")]
        [Authorize(Policy = "Passenger")]
        [HttpGet]
        public ActionResult<IEnumerable<OrderDTO>> GetHistory() {
            try {
                Passenger pas = GetPassenger();
                return _orderRepo.GetAllByPassenger(pas).Select(o => new OrderDTO(o)).ToList();
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("{id}")]
        [Authorize(Policy = "Passenger")]
        [HttpGet]
        public ActionResult<OrderDTO> GetOrder(int id) {
            try {
                Passenger pas = GetPassenger();
                Order ord = GetOrderObject(id);

                if(ord.PassengerId != pas.UserId)
                    throw new ArgumentException("You can't access someone elses order");

                return new OrderDTO(ord);
            }catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("{id}")]
        [Authorize(Policy = "Passenger")]
        [HttpDelete]
        public ActionResult<OrderDTO> DeleteOrder(int id) {
            try{
                Passenger pas = GetPassenger();
                Order ord = GetOrderObject(id);

                if (ord.PassengerId != pas.UserId)
                    throw new ArgumentException("You can't access someone elses order");

                if (ord.IsDone)
                    throw new ArgumentException("You can't delete a completed order");

                _orderRepo.Remove(ord);
                _orderRepo.SaveChanges();

                return new OrderDTO(ord);
            }catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "Passenger")]
        [HttpPost]
        public ActionResult PlaceOrder([FromBody] OrderLinePostDTO[] orderLines)
        {
            try{
                Passenger pas = GetPassenger();
                Order ord = new Order(false, DateTime.Now, pas);

                Array.ForEach(orderLines, (OrderLinePostDTO ol) =>{

                    Product prod = _productRepo.GetById(ol.ProductID);

                    if (prod == null)
                        throw new ArgumentNullException("We couldn't find the product you specified");

                    if (prod.Amount < ol.Amount) {
                        throw new ArgumentNullException("You cant order more items than we have in stock");
                    }

                    ord.AddOrderLine(prod, ol.Amount);
                });

                _orderRepo.Add(ord);
                _orderRepo.SaveChanges();

                return Created($"api/Order/passenger/{ord.OrderId}", ord);
            }catch (Exception e) {
                return BadRequest(e.Message);
            }

        }

        [Route("crew/approve/{id}")]
        [Authorize(Policy = "Crew")]
        [HttpPost]
        public ActionResult ApproveOrder(int id) {
            try {
                Order ord = _orderRepo.GetById(id);

                foreach (var item in ord.OrderLines) {
                    if (_productRepo.GetById(item.ProductId).Amount - item.Amount < 0) {
                        throw new ArgumentException("You can't order more item than we have in stock!");
                    }
                    _productRepo.GetById(item.ProductId).Amount -= item.Amount;
                }

                if (ord.IsDone)
                    throw new ArgumentException("You can't approve an order that has already been approved");
                    
                ord.IsDone = true;
                _orderRepo.SaveChanges();
                return Ok();
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("crew/deny/{id}")]
        [Authorize(Policy = "Crew")]
        [HttpPost]
        public ActionResult DenyOrder(int id) {
            try {
                Order ord = _orderRepo.GetById(id);
                if (ord.IsDone)
                    throw new ArgumentException("You can't delete a completed order");

                _orderRepo.Remove(ord);
                _orderRepo.SaveChanges();
                return Ok();
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Route("crew/orders")]
        [Authorize(Policy = "Crew")]
        [HttpGet]
        public ActionResult<IEnumerable<OrderDTO>> GetOrders() {
            return _orderRepo.GetAllUnprocessed().Select(o => new OrderDTO(o)).ToList();
        }


        private Order GetOrderObject(int id) {
            Order ord = _orderRepo.GetById(id);

            if (ord == null)
                throw new ArgumentNullException("We can't find the order you're looking for");

            return ord;
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