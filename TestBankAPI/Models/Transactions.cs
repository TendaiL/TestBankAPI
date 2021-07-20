﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBankAPI.Models.Data;

namespace TestBankAPI.Models
{
    public class Transactions : BaseModel
    {
        public string account_number { get; set; }
        public string account_type { get; set; }
        public decimal balanceBefore { get; set; }
        public decimal balanceAfter { get; set; }
        public DateTime DateTime { get; set; }
        public string transactionType { get; set; }

        public string status { get; set; }

    }
}
