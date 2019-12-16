using InFlightAppBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Repositories
{
    public interface ICrewMemberRepository
    {
        IEnumerable<CrewMember> GetAll();
        CrewMember GetById(int id);
        void Add(CrewMember crewMember);
        void Remove(CrewMember crewMember);
        void SaveChanges();
    }
}
