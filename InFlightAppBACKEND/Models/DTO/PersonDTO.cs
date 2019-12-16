using InFlightAppBACKEND.Models.Domain; 

//Only intended to be read from not edited
namespace InFlightAppBACKEND.Models.DTO{
    public class PersonDTO{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public PersonType Type { get; set; }

        public PersonDTO(User u){
            FirstName = u.FirstName;
            LastName = u.LastName;
            Id = u.UserId;

            Type = (u is CrewMember) ? PersonType.CrewMember : PersonType.Passenger; 
        }
    }
}
