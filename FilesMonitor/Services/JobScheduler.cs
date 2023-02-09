namespace FilesMonitor.Services
{
    public class JobScheduler :IJobScheduler
    {
        public void FireAndForgetJob() { Console.WriteLine("Fire & Forget Job"); }
        public void RecurringJob() { Console.WriteLine("Recurring Job"); }
        public void DelayedJob() { Console.WriteLine("Delayed Job"); }
        public void ContinuationJob() { Console.WriteLine("Continuation Job"); }
    }
}
