using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services;
using InFlightApp.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightApp.View_Model {
    class TravelGroupViewModel {
        public HubConnection hubConnection { get; private set;}
        public ObservableCollection<Message> Messages { get; private set; }
        private IEnumerable<Passenger> passengers;
        private Passenger current;

        private readonly ITravelGroupService _travelGroupService;
        private readonly IUserService _userService;

        public TravelGroupViewModel() {
            try
            {
                _travelGroupService = ServiceLocator.Current.GetService<ITravelGroupService>(true);
                _userService = ServiceLocator.Current.GetService<IUserService>(true);
                //TODO: Get seat from screen
                _userService.AuthenticatePassenger(10);
                _travelGroupService.ReloadHttpClient();
                _userService.ReloadHttpClient();
                getPassengers();
                Messages = new ObservableCollection<Message>();
                var pers = _userService.GetLoggedIn();
                current = passengers.SingleOrDefault(p => p.Id == pers.Id);
                getMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private async void getMessages() {
            var temp = await _travelGroupService.GetMessages();
            temp.ToList().ForEach(m => Messages.Add(m));
            InitilizeHub();
        }

        private void getPassengers() {
            passengers = _userService.GetPassengers();
        }


        private async void InitilizeHub() {
            hubConnection = new HubConnectionBuilder().WithUrl(ApiConnection.ChatURL).Build();
            await hubConnection.StartAsync();


            hubConnection.Closed += async (error) => {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };

            hubConnection.On<int, int, int, string, string>("ReceiveSentMessage", (travelgroupId, messageId, userId, date, message) => {
                if (current != null && current.TravelGroupId == travelgroupId) {
                    var sender = passengers.Where(p => p.TravelGroupId == travelgroupId && p.Id == userId).FirstOrDefault();
                    Messages.Add(new Message { MessageId = messageId, Sender = sender, Content = message, DateSent = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", null)});
                }
            });
        }

        public async void SendMessage(string message) {
            if (current != null) {
                await hubConnection.InvokeAsync("SendMessage", current.Id, message);
            }
        }
    }
}
