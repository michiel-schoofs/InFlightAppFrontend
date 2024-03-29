﻿using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace InFlightApp.Services.Repositories
{
    public class NotificationService : INotificationService
    {

        public IEnumerable<Notification> GetAllNotifications()
        {
            string url = $"{ApiConnection.URL}/Notification";
            string s = ApiConnection.Client.GetStringAsync(url).Result;
            JArray ar = JArray.Parse(s);

            return ar.Select(n =>
            {
                return new Notification()
                {
                    Content = n.Value<string>("content"),
                    Timestamp = n.Value<DateTime>("timestamp"),
                    Receiver = n.Value<string>("receiver")
                };
            }).ToList();
        }

        public Notification GetMostRecentNotification()
        {
            string url = $"{ApiConnection.URL}/Notification/recent";
            HttpResponseMessage s = ApiConnection.Client.GetAsync(url).Result;
            if (s.StatusCode == HttpStatusCode.OK)
            {
                string str = s.Content.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(str);

                return new Notification()
                {
                    Content = obj.Value<string>("content"),
                    Timestamp = obj.Value<DateTime>("timestamp"),
                    Receiver = obj.Value<string>("receiver")
                };
            }
            else { return null; }
        }

        public void SendNotification(string notification, string receiver)
        {
            string json = JsonConvert.SerializeObject(new { content = notification, receiver });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            ApiConnection.Client.PostAsync($"{ApiConnection.URL}/Notification", content);
        }
    }
}
