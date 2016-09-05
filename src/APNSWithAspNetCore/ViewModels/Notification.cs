using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APNSWithAspNetCore.ViewModels
{
    public class Notification
    {
        public string DeviceId { get; set; }
        public string Message { get; set; }
    }
}
