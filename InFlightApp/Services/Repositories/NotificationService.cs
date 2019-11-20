using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InFlightApp.Services.Repositories
{
    public class NotificationService : INotificationService
    {
        private HttpClient client;

        public NotificationService()
        {
            client = ApiConnection.Client;
        }

        public IEnumerable<Notification> GetAllNotifications()
        {
            string url = $"{ApiConnection.URL}/Notification";
            string s = client.GetStringAsync(url).Result;
            JArray ar = JArray.Parse(s);

            return ar.Select(n =>
            {
                return new Notification()
                {
                    Content = n.Value<string>("content"),
                    Timestamp = n.Value<DateTime>("timestamp")
                };
            }).ToList();
        }

        public Notification GetMostRecentNotification()
        {
            string url = $"{ApiConnection.URL}/Notification/recent";
            string s = client.GetStringAsync(url).Result;
            JObject obj = JObject.Parse(s);

            return new Notification()
            {
                Content = obj.Value<string>("content"),
                Timestamp = obj.Value<DateTime>("timestamp")
            };
        }

        public void SendNotification(string notification)
        {
            string json = JsonConvert.SerializeObject(new { content = notification });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.PostAsync($"{ApiConnection.URL}/Notification", content);
        }
    }
}
