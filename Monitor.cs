using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Humanizer.Bytes;

namespace DiskSpaceMonitor
{
    public class Monitor
    {
        private readonly System.Timers.Timer _timer;
        private readonly double _frequencyMilliseconds = 10000;
        public Monitor()
        {
            var drives = DriveInfo.GetDrives();
            foreach(var drive in drives)
            {
                if(drive.IsReady)
                {
                    Console.WriteLine($"Drive {drive.Name} has {drive.AvailableFreeSpace.Bytes().Humanize()} free of a total {drive.TotalSize.Bytes().Humanize()}");
                }
            }

            _timer = new System.Timers.Timer(_frequencyMilliseconds) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) =>
            {
                Console.WriteLine($"The date/time is: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            };
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
