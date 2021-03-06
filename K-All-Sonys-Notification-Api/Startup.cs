using K_All_Sonys_Notification_Api.DTO;
using ApplicationCore.Interfaces;
using Infraestructure.Services;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KAllSonysNotificationApi.ApplicationCore.Entities;
using KAllSonysNotificationApi.Infraestructure.Repositories;
using Infraestructure.Contexts;
using Microsoft.EntityFrameworkCore;
namespace K_All_Sonys_Notification_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NotificationContext> (options => options.UseMySql(Configuration.GetConnectionString("DbNotifications")).EnableDetailedErrors(true));
            services.AddControllers();
            services.AddTransient<IEmailService, EmailService>();
            var notificationMetadata =
           Configuration.GetSection("NotificationMetadata").
           Get<NotificationMetadata>();
            services.AddSingleton(notificationMetadata);
            services.AddSingleton(new SmtpClient());
            services.AddScoped<IAsyncRepository<Notification>, NotificationsRepository>();
            services.AddScoped<INotIficationService, NotificationService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { Title = "K_All_Sonys_Notification_Api", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Habilitar swagger
            app.UseSwagger();

            //indica la ruta para generar la configuraci?n de swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "K_All_Sonys_Notification_Api");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
