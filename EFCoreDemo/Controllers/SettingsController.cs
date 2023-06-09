using EFCoreDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EFCoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly ILogger<SettingsController> logger;

        public SettingsController(IOptions<AppSettings> appSettings, ILogger<SettingsController> logger)
        {
            this.appSettings = appSettings;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            this.logger.LogTrace(1, "Getting settings");
            this.logger.LogDebug(2, "Getting settings");
            this.logger.LogInformation(3, "Getting settings");
            this.logger.LogInformation(3, "SiteName = " + appSettings.Value.SiteName);
            this.logger.LogInformation(3, "SMTPIP = " + appSettings.Value.SMTPIP);
            this.logger.LogWarning(4, "Getting settings");
            this.logger.LogError(5, "Getting settings");
            this.logger.LogCritical(6, "Getting settings");

            return Ok(appSettings.Value);
        }
    }
}
