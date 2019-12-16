using InFlightAppBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Data.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetAll();
        Notification GetById(int id);
        Notification GetMostRecent();
        void Add(Notification notification);
        void Remove(Notification notification);
        void SaveChanges();
    }
}
