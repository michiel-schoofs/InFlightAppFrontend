using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services;
using InFlightApp.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace InFlightApp.View_Model {
    class TravelGroupViewModel {
        public HubConnection hubConnection { get; private set;}
        public ObservableCollection<Message> messages { get; private set; }
    private readonly ITravelGroupService _travelGroupService;

        public TravelGroupViewModel() {
            try
            {
                _travelGroupService = ServiceLocator.Current.GetService<ITravelGroupService>(true);
                messages = new ObservableCollection<Message>();
                //_travelGroupService.GetMessages().
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            InitilizeHub();
        }


        private async void InitilizeHub() {
            hubConnection = new HubConnectionBuilder().WithUrl(ApiConnection.ChatURL).Build();
            await hubConnection.StartAsync();


            hubConnection.Closed += async (error) => {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };

            hubConnection.On<int, int, int, string, string>("ReceiveSentMessage", async (travelgroupId, messageId, userId, date, message) => {
                //TODO: haal persoon op met naam etc.
                //TODO: check of current in travelgroup zit!
                Persoon persoon = new Persoon { Id = userId };
                messages.Add(new Message { MessageId = messageId, Sender = persoon, Content = message, DateSent = DateTime.Parse(date)});
                //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //    BroadcastResults.Text += message
                //);
            });
        }

        public async void SendMessage(string message) {
            //TODO: 3 vervangen door travelGroupId
            await hubConnection.InvokeAsync("SendMessage", 3, message);
        }
    }
}
