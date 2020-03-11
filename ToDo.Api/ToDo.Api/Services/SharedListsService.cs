using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDo.Infrastructure;

namespace ToDo.Api.Services
{
    public class SharedListsService : IHostedService, IDisposable
    {

        private ToDoDbContext _dbContext;
        private Timer _timer;

        private readonly ILogger<SharedListsService> _logger;
        private IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        public SharedListsService(ILogger<SharedListsService> logger, IServiceScopeFactory svcScopeFactory, IConfiguration configuration)
        {
            this._logger = logger;
            this._serviceScopeFactory = svcScopeFactory;
            this._configuration = configuration;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shared lists service starting...");

            int executionTime = Int32.Parse(_configuration["SharedListsExecutionTime"]); //extract from options

            _timer = new Timer(CheckForExpirations, null, TimeSpan.Zero, TimeSpan.FromSeconds(executionTime));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shared lists service ended.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }


        public void CheckForExpirations(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _dbContext = scope.ServiceProvider.GetService<ToDoDbContext>();

                DateTime currTime = DateTime.Now;

                _dbContext.ToDoSharedLists
                    .ToList()
                    .ForEach(share =>
                    {
                        //if the list has been shared for longer than 2 hours, it has expired
                        if((currTime - share.TimeOfSharing).TotalHours > 0.5)
                        {
                            //_logger.LogInformation($"The sharing time for todo list '{share.ToDoList.Title}' has expired");
                            _dbContext.ToDoSharedLists.Remove(share);
                        }
                    });

                _dbContext.SaveChanges();

            }
        }


    }
}
