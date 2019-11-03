using InFlightApp.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Repositories
{
    public class UserInterface : IUserInterface{
        private HttpClient client;

        public UserInterface() {
            client = ApiConnection.Client;
        }

        public bool Login(string username, string password){
            string json = JsonConvert.SerializeObject(new { username = username, password = password});
            var content = new StringContent(json , Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync($"{ApiConnection.URL}/Users/crewmember/login", content).Result;

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            return true;
        }
    }
}
