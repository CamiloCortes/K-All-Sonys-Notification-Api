using KAllSonysNotificationApi.ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infraestructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KAllSonysNotificationApi.Infraestructure.Repositories
{
    public class NotificationsRepository : IAsyncRepository<Notification>
    {
        protected readonly NotificationContext _dbContext;

        public NotificationsRepository(NotificationContext dbContext) {
            _dbContext = dbContext;
        }
        public async Task<Notification> AddAsync(Notification entity)
        {
          await _dbContext.Notifications.AddAsync(entity);
          await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
