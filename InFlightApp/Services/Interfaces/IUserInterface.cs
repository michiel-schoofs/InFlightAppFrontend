using InFlightApp.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace InFlightApp.Services.Interfaces
{
    public interface IUserInterface
    {
        bool Login(string username, string password);
        void StoreCredentials(string username, string password);
        PasswordCredential GetCredential();
        void RemoveCredential(PasswordCredential cred);

        Task<IEnumerable<Passenger>> GetPassengers();
        Task ChangeSeat(int userId, int seatId);

        void SendNotification(string notification);
    }
}
