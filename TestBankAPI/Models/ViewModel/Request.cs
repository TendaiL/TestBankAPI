using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBankAPI.Models.ViewModel
{
    public class Request
    {
        public string account_number { get; set; }
        public string account_type { get; set; }
       // public decimal balance { get; set; }
        public decimal ammount { get; set; }
    }
}
