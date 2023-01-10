using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskSpaceMonitor
{
    public class RetentionPolicy
    {
        public string Path { get; set; }
        public int DeleteOlderThanDays { get; set; }
    }
}
