using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Api.UtilModels
{
    public class ToDoOptions
    {
        public string SendGridApiKey { get; set; }

        public int ReminderExecutionTime { get; set; }

        public string LinkToExpiredList { get; set; }

        public EmailModel EmailModel { get; set; }

        public ToDoOptions()
        {

        }
    }
}
