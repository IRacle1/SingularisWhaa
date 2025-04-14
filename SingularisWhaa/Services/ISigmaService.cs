using Hangfire;
using Hangfire.Server;

namespace SingularisWhaa.Services
{
    public class CleanTempDirectoryProcess : BackgroundService
    {
        public void Execute(BackgroundProcessContext context)
        {
            BackgroundJob.Enqueue()
            Directory.CleanUp(Directory.GetTempDirectory());
            context.Wait(TimeSpan.FromHours(1));
        }
    }
}
