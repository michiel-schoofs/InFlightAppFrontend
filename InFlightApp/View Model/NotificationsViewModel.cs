using InFlightApp.Configuration;
using InFlightApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.View_Model
{
    public class NotificationsViewModel
    {
        private readonly IUserInterface _userInterface;

        public NotificationsViewModel()
        {
            try
            {
                _userInterface = ServiceLocator.Current.GetService<IUserInterface>(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendNotification(string notification)
        {
            _userInterface.SendNotification(notification);
        }
    }
}
