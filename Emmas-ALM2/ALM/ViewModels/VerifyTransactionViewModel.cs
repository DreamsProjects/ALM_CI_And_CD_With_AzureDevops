using ALM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALM.ViewModels
{
    public class VerifyTransactionViewModel
    {
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }
    }
}
