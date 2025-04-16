namespace SingularisWhaa.Services.Abstractions
{
    public interface IPeriodicTask
    {
        public Task DoTask(CancellationToken cancellationToken);
    }
}
