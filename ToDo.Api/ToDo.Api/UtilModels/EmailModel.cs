using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SendGrid.Helpers.Mail;

namespace ToDo.Api.UtilModels
{
    public class EmailModel
    {
        public string Sender { get; set; }

        public string Subject { get; set; }

        public string BodyParagraph { get; set; }

        public string BodyAnchor { get; set; }


        public EmailModel() { }
    }
}
