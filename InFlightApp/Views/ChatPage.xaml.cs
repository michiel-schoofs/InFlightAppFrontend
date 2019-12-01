using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InFlightApp.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page {
        private HubConnection hubConnection;
        private IHubProxy hubProxy;

        public ChatPage() {
            this.InitializeComponent();
            InitilizeHub();
        }

        private async void InitilizeHub() {
            hubConnection = new HubConnection("http://localhost:51178/chatHub", false);
            hubProxy = hubConnection.CreateHubProxy("BroadcastHub");
            ServicePointManager.DefaultConnectionLimit = 10;
            await hubConnection.Start();
            Console.WriteLine("Initialized.");
            hubProxy.On<DateTime>("Broadcast", async data => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    BroadcastResults.Text += data.ToString()
                );
            });

            //hubConnection.Closed += async () => {
            //    await Task.Delay(new Random().Next(0, 5) * 1000);
            //    await hubConnection.Start();
            //};
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            await hubProxy.Invoke("SendMessage", "Me", "lqzdnlqdl");
        }
    }
}
