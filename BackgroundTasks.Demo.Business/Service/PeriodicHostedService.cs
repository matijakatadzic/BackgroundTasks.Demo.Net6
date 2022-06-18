using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks.Demo.Business.Service
{
    class PeriodicHostedService : BackgroundService
    {
        private readonly ILogger<PeriodicHostedService> _logger;
        private readonly SettingService _settingService;

        private int _executionCount = 0;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
        private readonly IServiceScopeFactory _factory;

        public PeriodicHostedService(ILogger<PeriodicHostedService> logger, SettingService settingService, IServiceScopeFactory factory)
        {
            _logger = logger;
            _settingService = settingService;   
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    if (_settingService.IsEnabled)
                    {
                        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                        SampleService sampleService = asyncScope.ServiceProvider.GetRequiredService<SampleService>();
                        await sampleService.DoSomethingAsync();
                        _executionCount++;
                        _logger.LogInformation(
                            $"Executed PeriodicHostedService - Count: {_executionCount}");
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Skipped PeriodicHostedService");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(
                        $"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
                }
            }
        }
    }
}
