using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Core.Model.Base
{
    public class AppSettings
    {
        public string TokenSecret { get; set; }

        public int TokenValidateInMinutes { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string YaghutUserServiceUrl { get; set; }

        public int YaghutTokenAliveInMinutes { get; set; }

        public string PaymentUsername { get; set; }

        public string PaymentPassword { get; set; }
        
        public bool MockRemoteEnable { get; set; }
    }
}
