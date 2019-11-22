﻿using InFlightApp.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace InFlightApp.Services.Interfaces
{
    public interface IUserService
    {
        bool Login(string username, string password);
        void StoreCredentials(string username, string password);
        PasswordCredential GetCredential();
        void RemoveCredential(PasswordCredential cred);

        IEnumerable<Passenger> GetPassengers();
        void ChangeSeat(int userId, int seatId);

    }
}