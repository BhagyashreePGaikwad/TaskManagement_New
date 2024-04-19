namespace TaskManagement_April_.Service.Implementation
{
   
        public class TaskReminderService : IHostedService, IDisposable
        {
            private readonly IServiceProvider _serviceProvider;
            private Timer _timer;

            public TaskReminderService(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public Task StartAsync(CancellationToken cancellationToken)
            {
            _timer = new Timer(CheckIncompleteTasksAndSendEmail, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            //return Task.CompletedTask;
                //TimeSpan interval = TimeSpan.FromMinutes(5000); // Adjust the interval as needed
                //_timer = new Timer(CheckIncompleteTasksAndSendEmail, null, TimeSpan.Zero, interval);
                return Task.CompletedTask;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                _timer?.Change(Timeout.Infinite, 0);
                return Task.CompletedTask;
            }

            private void CheckIncompleteTasksAndSendEmail(object state)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                    taskService.CheckIncompleteTasksAndSendEmail().Wait();
                }
            }

            public void Dispose()
            {
                _timer?.Dispose();
            }
        }
}
