using InFlightApp.Model;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Repositories {
    class TravelGroupRepository : ITravelGroupRepository {

        private HttpClient client;

        public TravelGroupRepository() {
            client = ApiConnection.Client;
        }

        public void AddMessage(string content) {
            throw new NotImplementedException();
        }

        public Task<Message[]> GetMessages() {
            throw new NotImplementedException();
        }
    }
}
