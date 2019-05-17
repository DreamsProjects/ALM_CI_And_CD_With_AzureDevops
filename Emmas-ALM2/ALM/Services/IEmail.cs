using ALM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALM.Services
{
   public interface IEmail
    {
        void EmailSender(Customer customer);
    }
}
