using ALM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALM.ViewModels
{
    public class HomeIndexViewModel
    {
        public string EnvironmentInfo { get; set; }
        public string MyKey { get; set; }
        public string EmailUsername { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
