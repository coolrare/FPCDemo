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

        public SettingsController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(appSettings.Value);
        }
    }
}
