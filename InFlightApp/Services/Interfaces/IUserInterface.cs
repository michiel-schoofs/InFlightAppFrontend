using Windows.Security.Credentials;

namespace InFlightApp.Services.Interfaces
{
    public interface IUserInterface{
        bool Login(string username, string password);
        void StoreCredentials(string username, string password);
        PasswordCredential GetCredential();
        void RemoveCredential(PasswordCredential cred);
    }
}
