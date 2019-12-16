using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace InFlightAppBACKEND.Data.Repositories
{
    public class CrewMemberRepository : ICrewMemberRepository
    {
        private DbSet<CrewMember> _crewMembers;
        private DBContext _dbContext;

        public CrewMemberRepository(DBContext dbContext)
        {
            _crewMembers = dbContext.CrewMembers;
            _dbContext = dbContext;
        }

        public IEnumerable<CrewMember> GetAll()
        {
            return _crewMembers.ToList();
        }

        public CrewMember GetById(int id)
        {
            return _crewMembers.SingleOrDefault(cm => cm.UserId == id);
        }

        public void Add(CrewMember crewMember)
        {
            _crewMembers.Add(crewMember);
        }

        public void Remove(CrewMember crewMember)
        {
            _crewMembers.Remove(crewMember);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
