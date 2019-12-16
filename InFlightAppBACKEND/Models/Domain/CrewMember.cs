using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.Domain
{
    public class CrewMember : User{
        //Normally would check if unique when registering but who cares now
        public string Username { get => $"{FirstName.Replace(' ','_')}.{LastName.Replace(' ', '_')}";  }

        protected CrewMember() : base() { }

        public CrewMember(string firstName, string lastName)
        : base(firstName, lastName) { }
    }
}
