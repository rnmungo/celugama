using CeluGamaSystem.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CeluGamaSystem.Services.Interfaces
{
    public interface INotificationService
    {
        void ProcessNotification(Notification notification);
    }
}
