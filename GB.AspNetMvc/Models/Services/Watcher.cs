using System.Diagnostics;

namespace GB.AspNetMvc.Models.Services
{
    public class Watcher : BackgroundService
    {
        private readonly ILogger<Watcher> _logger;

        public Watcher(ILogger<Watcher> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            Stopwatch sw = Stopwatch.StartNew();
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("{WorkTime} - полет нормальный!", sw.Elapsed);
            }
        }

    }
}
