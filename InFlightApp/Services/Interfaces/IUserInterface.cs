using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Services.Interfaces
{
    public interface IUserInterface{
        bool Login(string username, string password);
    }
}
