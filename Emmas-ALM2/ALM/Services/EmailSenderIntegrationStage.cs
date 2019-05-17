using ALM.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ALM.Services
{
    public class EmailSenderIntegrationStage : IEmail
    {
        private readonly IConfiguration _config;

        public EmailSenderIntegrationStage(IConfiguration config)
        {
            _config = config;
        }

        public void EmailSender(Customer customer)
        {
            var user = _config.GetValue<string>("Email:Username");
            var pass = _config.GetValue<string>("Email:Password");
            var host = _config.GetValue<string>("Email:HostName");
            var plainTextContent = $"Hej {customer.Name}! På ditt konto nummer {customer.Account.FirstOrDefault().AccountId} så har " +
                $"{customer.Account.FirstOrDefault().Money} på ditt konto./b Du får detta meddelandet från startsidan";

            var client = new SmtpClient(host, 2525)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            client.Send("Emma.yh@nackademin.se", "Kund@EmmasBank.se", "Saldo information", plainTextContent);
        }
    }
}
