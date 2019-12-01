using Microsoft.AspNetCore.SignalR.Client;
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

        public ChatPage() {
            this.InitializeComponent();
            InitilizeHub();
        }

        private async void InitilizeHub() {
            hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:51178/chatHub").Build();
            hubConnection.On<string, string>("SendMessage", async (user, message) => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    BroadcastResults.Text += message
                );
            });

            await hubConnection.StartAsync();


            hubConnection.Closed += async (error) => {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            await hubConnection.InvokeAsync("SendMessage", "Me", "lqzdnlqdl");
        }
    }
}
