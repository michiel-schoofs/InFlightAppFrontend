using InFlightAppBACKEND.Data.Repositories;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ITravelGroupRepository _travelGroupRepository;
        private readonly IPassengerRepository _passengerRepository;

        public ChatHub(ITravelGroupRepository travelGroupRepository, IPassengerRepository passengerRepository)
        {
            _travelGroupRepository = travelGroupRepository;
            _passengerRepository = passengerRepository;
        }

        public async Task SendMessage(int userId, string content)
        {
            Passenger passenger = _passengerRepository.GetById(userId);
            Message message = _travelGroupRepository.AddMessage(passenger.TravelGroup.TravelGroupId, passenger, content);
            _travelGroupRepository.SaveChanges();
            await Clients.All.SendAsync("ReceiveSentMessage", passenger.TravelGroup.TravelGroupId, message.MessageId, passenger.UserId, message.DateSent.ToString("dd/MM/yyyy HH:mm:ss"), content);
        }
    }
}