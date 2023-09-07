using Microsoft.AspNetCore.SignalR;

namespace webapi.Service
{
    public class TimeIntervalService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IHubContext<PollNotificationHub, INotificationHub> _pollNotification;

        public TimeIntervalService(ILogger<TimeIntervalService> logger, IHubContext<PollNotificationHub, INotificationHub> hubContext)
        {
            _logger = logger;
            _pollNotification = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Service is running.");
            if (_pollNotification != null)
            {
                _pollNotification.Clients.All.SendMessage("poll");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
