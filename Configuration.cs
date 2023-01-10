using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskSpaceMonitor
{
    public class Configuration
    {
        public Configuration()
        {
            RetentionPolicies = new List<RetentionPolicy>();
            MonitoredVolumes = new List<Volume>();
        }

        public List<RetentionPolicy> RetentionPolicies { get; set; }

        public List<Volume> MonitoredVolumes { get; set; }
    }
}
