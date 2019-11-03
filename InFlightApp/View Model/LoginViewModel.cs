using InFlightApp.Configuration;
using InFlightApp.Services;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model{
    public class LoginViewModel{
        public bool RememberMe { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public readonly IUserInterface _userRepo;

        public LoginViewModel(){
            try{
                _userRepo = ServiceLocator.Current.GetService<IUserInterface>(true);
            }catch (Exception e) {
                Console.WriteLine(e);
            }

            Console.WriteLine(_userRepo);

            RememberMe = false;
            Username = "";
            Password = "";
        }


    }
}
