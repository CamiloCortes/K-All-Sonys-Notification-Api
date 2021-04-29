using ApplicationCore.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface INotIficationService
    {
        public Task<Notification> CreateNotificationAsync(Notification notification);
    }
}
