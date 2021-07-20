using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestBankAPI.Models;

namespace TestBankAPI.Helper
{
    public static class Seed
    {
        public static void SeedData(BankContext context)
        {
            SeedAccounts(context);
        }

        private static void SeedAccounts(BankContext context)
        {
            var Type = context.Accounts.FirstOrDefault();
            if (Type == null)
            {
                try
                {
                    context.Accounts.AddRange(
                               new Accounts
                               {
                                   account_number = "1222828882828282",
                                   account_type = "Savings" ,
                                   balance = 13000
                               },
                               new Accounts
                               {
                                   account_number = "6363737373738383",
                                   account_type = "Savings",
                                   balance = 12000,
                                   
                               },
                               new Accounts
                               {
                                   account_number = "1363937373738389",
                                   account_type = "Savings",
                                   balance = 4100,
                                   
                               },
                               new Accounts
                               {
                                   account_number = "4363737573738388",
                                   account_type = "Savings",
                                   balance = 2900,                                 
                               }
                     );
                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}