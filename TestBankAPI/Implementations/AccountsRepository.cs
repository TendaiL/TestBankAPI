using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBankAPI.Interfaces;
using TestBankAPI.Models;

namespace TestBankAPI.Implementations
{
    public class AccountsRepository : BaseRepository<Accounts>, IAccount
    {
        private BankContext context;
        public AccountsRepository(BankContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }
    }
}
