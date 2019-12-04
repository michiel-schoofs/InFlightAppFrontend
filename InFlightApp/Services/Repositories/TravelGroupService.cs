using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Repositories {
    class TravelGroupService : ITravelGroupService {

        private HttpClient client;

        public TravelGroupService() {
            client = ApiConnection.Client;
        }

        public void ReloadHttpClient() {
            client = ApiConnection.Client;
        }

        public bool AddMessage(string content) {
            HttpResponseMessage response = client.PostAsync($"{ApiConnection.URL}/TravelGroup/messages?content={content}", null).Result;

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            return true;
        }

        public async Task<Message[]> GetMessages() {
            string url = $"{ApiConnection.URL}/TravelGroup/messages";
            string response = await client.GetStringAsync(url);
            JArray responseArray = JArray.Parse(response);

            if (responseArray != null)
            {
                return responseArray.Select(e => {
                    var token = e.Value<JToken>("sender");
                    JObject persoonObj = JObject.Parse(token.ToString());
                    Persoon p = new Persoon { 
                        Id = persoonObj.Value<int>("id"),
                        FirstName = persoonObj.Value<string>("firstName"),
                        LastName = persoonObj.Value<string>("lastName")
                    };
                    return new Message
                    {
                        MessageId = e.Value<int>("messageId"),
                        Content = e.Value<string>("content"),
                        DateSent = DateTime.ParseExact(e.Value<string>("dateSent"), "dd/MM/yyyy HH:mm:ss", null),
                        Sender = p
                    };
                }).ToArray();
            }
            throw new Exception("Could not parse Object.");
        }
    }
}
