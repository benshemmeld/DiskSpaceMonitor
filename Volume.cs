using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskSpaceMonitor
{
    public class Volume
    {
        public string Name { get; set; }
        public long BytesRemainingWarningThreshold { get; set; }

        public string Email { get; set; }
    }
}
