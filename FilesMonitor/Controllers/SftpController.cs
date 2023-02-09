using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FilesMonitor.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class SftpController : ControllerBase
    {
        private readonly ISftpBusiness _sftpBusiness;
        
        public SftpController(ISftpBusiness sftpBusiness)
        {
            _sftpBusiness = sftpBusiness;
        }
        [HttpGet]
        public async Task Monitor()
        {
            await _sftpBusiness.MonitorFolder();
        }
    }
}
