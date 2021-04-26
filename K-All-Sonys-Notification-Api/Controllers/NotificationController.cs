using K_All_Sonys_Notification_Api.DTO;
using K_All_Sonys_Notification_Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K_All_Sonys_Notification_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public NotificationController(IEmailService EmailService)
        {

            _emailService = EmailService;
        }

        [Consumes("application/json")]
        [Route("email")]
        [HttpPost]
        public async Task<IActionResult> sendEmail(EmailMessage message)
        {
            return Ok( await _emailService.SendEmail(message));
        }
    }
}
