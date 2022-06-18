using BackgroundTasks.Demo.Business.Service;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundTasks.Demo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly SettingService _settingService;

        public SettingsController(ILogger<SettingsController> logger, SettingService settingService)
        {
            _logger = logger;
            _settingService = settingService;
        }

        [HttpGet(Name = "EnableBackgroundProcess")]
        public IActionResult Get(bool isEnabled)
        {
            _settingService.IsEnabled = isEnabled;
            return Ok();
        }
    }
}