using InFlightAppBACKEND.Data.Repositories.Interfaces;
using InFlightAppBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private DbSet<Notification> _notifications;
        private DBContext _dbContext;

        public NotificationRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _notifications = dbContext.Notifications;
        }

        public void Add(Notification notification)
        {
            _notifications.Add(notification);
        }

        public IEnumerable<Notification> GetAll()
        {
            return _notifications.ToList();
        }

        public Notification GetById(int id)
        {
            return _notifications.SingleOrDefault(n => n.NotificationId == id);
        }

        public Notification GetMostRecent()
        {
            return _notifications.OrderByDescending(n => n.Timestamp).First();
        }

        public void Remove(Notification notification)
        {
            _notifications.Remove(notification);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
