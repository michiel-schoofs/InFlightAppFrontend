using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public abstract class User
    {
        public Image ProfilePicture { get; set; }
        public int? ProfilePictureID { get; set; }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        protected User() { }

        protected User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public virtual void AddOrder(Order order) { }
        public virtual void RemoveOrder(Order order) { }
    }
}
