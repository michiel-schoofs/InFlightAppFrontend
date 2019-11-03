using InFlightApp.Configuration;
using InFlightApp.Services.Interfaces;
using System;
using Windows.Security.Credentials;

namespace InFlightApp.View_Model{
    public class MainPageViewModel {
        IUserInterface _userRepo;

        public delegate void LoginGrantedDelegate();
        public event LoginGrantedDelegate LoginGranted;

        public MainPageViewModel() {
            try{
                _userRepo = ServiceLocator.Current.GetService<IUserInterface>(true);
            }catch (Exception e){
                //Replace with logging later on
                Console.WriteLine(e);
            }
        }

        public void CheckForUserCredentials() {
            PasswordCredential cred = _userRepo.GetCredential();

            if (cred != null){
                bool res = _userRepo.Login(cred.UserName, cred.Password);

                if (!res) 
                    _userRepo.RemoveCredential(cred);
                else
                    LoginGranted.Invoke();
            }
        }

    }
}
