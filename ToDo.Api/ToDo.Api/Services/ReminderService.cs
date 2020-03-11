using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ToDo.Infrastructure;
using ToDo.Api.UtilModels;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace ToDo.Api.Services
{
    public class ReminderService : IHostedService, IDisposable
    {

        private ToDoDbContext _dbContext;
        private Timer _timer;

        private readonly ToDoOptions _options;
        private readonly SendGridClient _emailClient;

        private readonly ILogger<ReminderService> _logger;
        private IServiceScopeFactory _serviceScopeFactory;
        

        public ReminderService(ILogger<ReminderService> logger, IServiceScopeFactory svcScopeFactory, IOptions<ToDoOptions> options)
        {
            _logger = logger;
            _serviceScopeFactory = svcScopeFactory;
            _options = options.Value;
            _emailClient = new SendGridClient(_options.SendGridApiKey);
        }


        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder service starting...");

            int executionTime = _options.ReminderExecutionTime; //extract from options

            _timer = new Timer(CheckForExpirations, null, TimeSpan.Zero, TimeSpan.FromSeconds(executionTime));

            return Task.CompletedTask;

        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder service ended.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }


        private void CheckForExpirations(object state)
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _dbContext = scope.ServiceProvider.GetService<ToDoDbContext>();

                DateTime today = DateTime.Today;

                //loop through to do lists and check if their reminder date expired
                _dbContext.ToDoLists
                    .ToList()
                    .ForEach(list =>
                    {
                        if (list.IsReminded && list.EndDate.HasValue && today > list.EndDate.Value)
                        {
                            _logger.LogInformation($"ReminderService -> Found expired list with title '{list.Title}'");

                            SendEmail($"{_options.LinkToExpiredList}/{list.Id}", list.Owner);
                            list.IsReminded = false;
                        }
                    });

                 _dbContext.SaveChanges();

            }

        }


        private void SendEmail(string linkToList, string receiverEmail)
        {

            //creating body from options attributes
            string emailBody = string.Concat(_options.EmailModel.BodyParagraph, linkToList, _options.EmailModel.BodyAnchor);

            var msg = MailHelper.CreateSingleEmail(new EmailAddress(_options.EmailModel.Sender), new EmailAddress(receiverEmail),
               _options.EmailModel.Subject, "", emailBody);

            _emailClient.SendEmailAsync(msg);
           

        }


    }
}
