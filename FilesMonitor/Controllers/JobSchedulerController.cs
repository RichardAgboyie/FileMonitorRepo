using BusinessLogic.Interfaces;
using FilesMonitor.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using BackgroundJobClientExtensions = Hangfire.BackgroundJobClientExtensions;
using IBackgroundJobClient = Hangfire.IBackgroundJobClient;
using RecurringJobManagerExtensions = Hangfire.RecurringJobManagerExtensions;

namespace FilesMonitor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSchedulerController : ControllerBase
    {
        private readonly ISftpBusiness _sftpBusiness;
        private readonly IJobScheduler _jobScheduler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;


        /// <summary>
        /// constructor
        /// </summary>
        public JobSchedulerController(ISftpBusiness sftpBusiness, IJobScheduler jobScheduler, IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
        {
            _sftpBusiness = sftpBusiness;
            _jobScheduler = jobScheduler;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }
        // GET: api/<JobSchedulerController/StartJob>
        [HttpGet]
        public string StartJob()
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Job Scheduler Started"));
            return $"JOB ID : {jobId} started.";
        }

        // GET: api/<JobSchedulerController>
        [HttpGet]
        public string ScheduleJob()
        {
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Job Scheduled to monitor folder"), TimeSpan.FromSeconds(30));
            return $"JOB ID : {jobId} started.";
        }

        // GET: api/<JobSchedulerController>
        [HttpGet]
        public string DailyMonitorJob()
        {
            RecurringJob.AddOrUpdate(() => _sftpBusiness.MonitorFolder(), Cron.Daily);
            return "Folder Monitored";
        }

        [HttpGet("CreateFireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob() 
        {
            _backgroundJobClient.Enqueue(() => _jobScheduler.FireAndForgetJob());
            return Ok();
        }

        [HttpGet("MonitorFolder")]
        public ActionResult MonitorFolder()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _sftpBusiness.MonitorFolder(), Cron.Minutely);
                return Ok();
        }

      
    }
}
