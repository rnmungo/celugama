using System;
using Microsoft.AspNetCore.Mvc;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Services.Interfaces;

namespace CeluGamaSystem.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpPost]
        public void PostNotification([FromBody] Notification notification)
        {
            _service.ProcessNotification(notification);
        }
    }
}