using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALM.Models
{
    public class Customer
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<Account> Account { get; set; }
    }
}
