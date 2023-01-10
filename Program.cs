using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Topshelf;

namespace DiskSpaceMonitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args).Build();
            var config = host.Services.GetRequiredService<IConfiguration>();
            var smtpConfig = config.GetSection("Smtp");

            var topShelfExitCode = HostFactory.Run(x =>
            {
                x.Service<Monitor>(s =>
                {
                    s.ConstructUsing(name => new Monitor(smtpConfig));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Disk Space Monitor and Retention Policy Enforcer");
                x.SetDisplayName("Disk Space Monitor");
                x.SetServiceName("Disk Space Monitor");
            });

            var exitCode = (int)Convert.ChangeType(topShelfExitCode, topShelfExitCode.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}