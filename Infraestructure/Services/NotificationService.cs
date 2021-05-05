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
        private IAsyncRepository<KAllSonysNotificationApi.ApplicationCore.Entities.Notification> _repository;

        public NotificationService(IAsyncRepository<KAllSonysNotificationApi.ApplicationCore.Entities.Notification> repository) {

            _repository = repository;
        }
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            var newNotification = new KAllSonysNotificationApi.ApplicationCore.Entities.Notification
            {
                Message = notification.Message,
                Status = notification.status,
                Type = notification.Type,
                Reciever = notification.Reciever,
                DateNotification = notification.DateNotification
            };

            newNotification = await _repository.AddAsync(newNotification);
            notification.Id = newNotification.Id;

            return notification;
        }
    }
}
