using InFlightApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Interfaces {
    interface ITravelGroupRepository {
        Task<Message[]> GetMessages();
        bool AddMessage(string content);
    }
}
