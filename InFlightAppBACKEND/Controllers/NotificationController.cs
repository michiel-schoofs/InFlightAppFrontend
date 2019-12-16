using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InFlightAppBACKEND.Data.Repositories.Interfaces;
using InFlightAppBACKEND.Models.Domain;
using InFlightAppBACKEND.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InFlightAppBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificiationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificiationRepository = notificationRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Notification>> GetAllNotifications()
        {
            IEnumerable<Notification> notifications = _notificiationRepository.GetAll().ToList();
            if (notifications == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(notifications);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Notification> GetNotification(int id)
        {
            return _notificiationRepository.GetById(id);
        }

        [HttpGet]
        [Route("recent")]
        public ActionResult<Notification> GetMostRecentNotification()
        {
            Notification notification;
            try
            {
                notification = _notificiationRepository.GetMostRecent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            if (notification == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(notification);
            }
        }

        [HttpPost]
        public ActionResult<Notification> SendNotification(NotificationDTO model)
        {
            Notification notification = new Notification(model.Content,model.Receiver);
            _notificiationRepository.Add(notification);
            _notificiationRepository.SaveChanges();
            return notification;
        }
    }
}