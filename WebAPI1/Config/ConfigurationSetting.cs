using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI1.Config
{
    public class ConfigurationSetting
    {
        public string ServiceName { get; set; }
        public string ServiceHost { get; set; }
        public int ServicePort { get; set; }
        public string ConsulAddresss { get; set; }
    }
}
