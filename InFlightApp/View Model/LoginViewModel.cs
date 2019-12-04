using InFlightApp.Configuration;
using InFlightApp.Model;
using InFlightApp.Services;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model{
    public class LoginViewModel {
        public bool RememberMe { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public readonly IUserService _userRepo;
        public RelayCommand Login { get; set; }

        public delegate Task LoginFailedDelegate(string str);
        public event LoginFailedDelegate LoginFailedEvent;

        public delegate void LoginSuccessDelegate();
        public event LoginSuccessDelegate LoginSuccess;

        public LoginViewModel() {
            try {
                _userRepo = ServiceLocator.Current.GetService<IUserService>(true);
            } catch (Exception e) {
                //Replace with logging later on
                Console.WriteLine(e);
            }

            RememberMe = false;
            Username = "";
            Password = "";

            ConfigureCommands();
        }

        private void ConfigureCommands() {
            Login = new RelayCommand(_ => LoginToApplication());
        }

        public void LoginToApplication() {
            bool success = _userRepo.Login(Username, Password);

            if (!success)
                LoginFailedEvent.Invoke("Username or password incorrect");
            else{
                LoginSuccess?.Invoke();
                if (RememberMe)
                    _userRepo.StoreCredentials(Username, Password);
            }
        }

        public Passenger GetLoggedIn()
        {
            return _userRepo.GetLoggedIn();
        }

    }
}
