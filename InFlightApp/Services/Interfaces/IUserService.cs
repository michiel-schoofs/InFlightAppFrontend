﻿using InFlightApp.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace InFlightApp.Services.Interfaces
{
    public interface IUserService{
        bool Login(string username, string password);
        void LogOut();
        bool AuthenticatePassenger(int seatnumber);

        void ReloadHttpClient();
        void StoreCredentials(string username, string password);
        PasswordCredential GetCredential();
        void RemoveCredential(PasswordCredential cred);

        Seat GetSeatOfLogedIn();
        Passenger GetLoggedIn();
        PassengerType? GetpassengerType();

        Task<bool> HasImage();
        Task<string> GetImage();
        Task<string> GetImageForPerson(Persoon pers);

        Task<bool> HasTravelgroup();

        IEnumerable<Passenger> GetPassengers();
        void ChangeSeat(int userId, int seatId);

    }
}
