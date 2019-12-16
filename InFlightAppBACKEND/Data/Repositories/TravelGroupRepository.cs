using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace InFlightAppBACKEND.Data.Repositories
{
    public class TravelGroupRepository : ITravelGroupRepository
    {
        private DbSet<TravelGroup> _travelGroups;
        private DBContext _dbContext;

        public TravelGroupRepository(DBContext dbContext)
        {
            _travelGroups = dbContext.TravelGroups;
            _dbContext = dbContext;
        }

        public IEnumerable<TravelGroup> GetAll()
        {
            return _travelGroups
                .Include(t => t.Conversation)
                .Include(t => t.Passengers)
                .ToList();
        }

        public IEnumerable<Message> GetMessagesFromTravelGroup(int id)
        {
            //return _travelGroups.SingleOrDefault(t => t.TravelGroupId == id).Conversation.Messages.OrderBy(e => e.DateSent).ToList();
            return _travelGroups.Include(tg => tg.Conversation).ThenInclude(c => c.Messages).SingleOrDefault(t => t.TravelGroupId == id).Conversation.Messages.OrderBy(e => e.DateSent).ToList();
        }

        public IEnumerable<Passenger> GetPassengersFromTravelGroup(int id)
        {
            return _travelGroups.SingleOrDefault(t => t.TravelGroupId == id).Passengers.ToList();
        }

        public Message AddMessage(int travelGroupId, Passenger passenger, string content) {
            var message = GetById(travelGroupId).sendMessage(passenger, content);
            SaveChanges();
            return message;
        }

        public TravelGroup GetById(int id)
        {
            return _travelGroups
                .Include(t => t.Conversation).ThenInclude(c => c.Messages)
                .Include(t => t.Passengers)
                .SingleOrDefault(t => t.TravelGroupId == id);
        }

        public void Add(TravelGroup travelGroup)
        {
            _travelGroups.Remove(travelGroup);
        }

        public void Remove(TravelGroup travelGroup)
        {
            _travelGroups.Remove(travelGroup);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
