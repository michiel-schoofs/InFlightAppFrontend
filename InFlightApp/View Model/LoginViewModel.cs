using InFlightApp.Configuration;
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
        public readonly IUserInterface _userRepo;
        public RelayCommand Login { get; set; }

        public LoginViewModel() {
            try {
                _userRepo = ServiceLocator.Current.GetService<IUserInterface>(true);
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
            _userRepo.Login(Username, Password);
        }

    }
}
