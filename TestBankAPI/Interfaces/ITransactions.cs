using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TestBankAPI.Implementations;
using TestBankAPI.Models;

namespace TestBankAPI.Interfaces
{
    public interface ITransactions : IRepository<Transactions>
    {

    }
}
