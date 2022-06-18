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
        /// <summary>
        /// GetIsEnabled
        /// </summary>
        /// <returns>IsEnabled</returns>
        [HttpGet(Name = "GetIsEnabled")]
        public IActionResult GetIsEnabled()
        {
            return Ok(_settingService.IsEnabled);
        }

        /// <summary>
        /// UpdateIsEnabled
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        [HttpPost(Name = "UpdateIsEnabled")]
        public IActionResult UpdateIsEnabled(bool isEnabled)
        {
            _settingService.IsEnabled = isEnabled;
            return Ok();
        }
    }
}