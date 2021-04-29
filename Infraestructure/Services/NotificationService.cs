using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services
{
    public class NotificationService : INotIficationService
    {
        private IAsyncRepository<ApplicationCore.Entities.Notification> _repository;

        public NotificationService(IAsyncRepository<ApplicationCore.Entities.Notification> repository) {

            _repository = repository;
        }
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            var newNotification = new ApplicationCore.Entities.Notification
            {
                Message = notification.Message,
                Status = notification.status,
                Type = notification.Type
            };

            newNotification = await _repository.AddAsync(newNotification);
            notification.Id = newNotification.Id;

            return notification;
        }
    }
}
