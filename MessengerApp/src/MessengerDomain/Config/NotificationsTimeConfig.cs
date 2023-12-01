using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerDomain.Config
{
    public class NotificationsTimeConfig
    {
        public NotificationsTimeConfig() { 
        
        }

        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
