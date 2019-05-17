using ALM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using SendGrid;

namespace ALM.Services
{
    public class EmailSenderProduction : IEmail
    {
        private readonly IConfiguration _config;

        public EmailSenderProduction(IConfiguration config)
        {
            _config = config;
        }
        

        public void EmailSender(Customer customer)
        {
            var apikey = _config.GetValue<string>("Email:ApiSendGrid");

            var client = new SendGridClient(apikey);
            var from = new EmailAddress("Emma.Karlsson@yh.nackademin.se", "Banken");
            var subject = "Ditt konto";

            var to = new EmailAddress($"{customer.Name}.{customer.PersonId}@EmmasBank.se");
            var plainTextContent = $"Hej {customer.Name}! På ditt konto nummer {customer.Account.FirstOrDefault().AccountId} så har {customer.Account.FirstOrDefault().Money} på ditt personkonto./b"+
                "Du får detta meddelandet från startsidan";
            var htmlContent = $"<strong>{plainTextContent}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var reponse = client.SendEmailAsync(msg);
        }
    }
}
