using System.Net.Mail;
using System.Text.Json;
using Humanizer;

namespace DiskSpaceMonitor
{
    public class Monitor
    {
        private readonly System.Timers.Timer _timer;
        private readonly double _frequencyMilliseconds = 1000 * 60 * 60; //1 hour

        public Monitor()
        {
            _timer = new System.Timers.Timer(_frequencyMilliseconds) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) =>
            {
                _timer.Enabled = false;
                
                Process();

                _timer.Interval = _frequencyMilliseconds;
                _timer.Enabled = true;
            };
        }

        public void Start()
        {
            LoadConfiguration();

            //Start with an interval of just 1 second, so it will process once and then revert to the configured frequency
            _timer.Interval = 1000;
            _timer.Start();
        }

        private void LoadConfiguration()
        {
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration.json");
            if (File.Exists(configFilePath))
            {
                var configJson = File.ReadAllText(configFilePath);
                Configuration = JsonSerializer.Deserialize<Configuration>(configJson);
            }
            else
            {
                //configuration.json doesn't exist - create it with some sample config
                Configuration = new Configuration();
                Configuration.MonitoredVolumes.Add(new Volume
                {
                    Name = @"C:\",
                    BytesRemainingWarningThreshold = 10 * 1024 * 1024, //10MB,
                    Email = "ben.shemmeld@uhg.com.au"
                });

                Configuration.RetentionPolicies.Add(new RetentionPolicy
                {
                    Path = @"d:\temp",
                    DeleteOlderThanDays = 60
                });
                var configJson = JsonSerializer.Serialize(Configuration);
                File.WriteAllText(configFilePath, configJson);
            }
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public Configuration Configuration { get; set; }

        private void Process()
        {
            foreach (var retentionPolicy in Configuration.RetentionPolicies)
            {
                retentionPolicy.Enforce(DateTime.Now);
            }

            foreach (var volume in Configuration.MonitoredVolumes)
            {
                var driveInfo = DriveInfo.GetDrives().FirstOrDefault(x => x.Name.ToLower() == volume.Name.ToLower());
                if (driveInfo == null)
                {
                    continue;
                }

                if (driveInfo.AvailableFreeSpace < volume.BytesRemainingWarningThreshold)
                {
                    var message = $"{volume.Name} has less than {driveInfo.AvailableFreeSpace.Bytes().Humanize()} available free space";
                    Console.WriteLine(message);
                    SendDiskSpaceWarningEmail(volume.Email, message);
                }
            }
        }

        private void SendDiskSpaceWarningEmail(string email, string message)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Send("ben.shemmeld@uhg.com.au", email, message, message);
            }
        }
    }
}
