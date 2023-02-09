namespace FilesMonitor.Services
{
    public interface IJobScheduler
    {
        void FireAndForgetJob();
        void RecurringJob();
        void DelayedJob();
        void ContinuationJob();
    }
}
