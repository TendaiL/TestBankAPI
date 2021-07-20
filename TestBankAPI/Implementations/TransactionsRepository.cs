using TestBankAPI.Interfaces;
using TestBankAPI.Models;

namespace TestBankAPI.Implementations
{
    public class TransactionsRepository : BaseRepository<Transactions>, ITransactions
    {
        private BankContext context;
        public TransactionsRepository(BankContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }
    }
}
