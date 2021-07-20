using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestBankAPI.Interfaces;
using TestBankAPI.Models;
using TestBankAPI.Models.ViewModel;

namespace TestBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ITransactions _transactions;
        private readonly IAccount _accounts;
        public AccountsController(BankContext context, IAccount accounts, ITransactions transactions)
        {
            _accounts = accounts;
            _transactions = transactions;
        }


        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreateAccounts([FromBody] Request request)
        {
            try
            {
                if (request.account_type.ToLower() == "savings")
                {
                    if (request.ammount >= 1000)
                    {
                        await _accounts.Add(
                             new Accounts
                             {
                                 account_number = request.account_number,
                                 account_type = request.account_type,
                                 balance  = request.ammount
                             });
                        return Ok(new { Success = true, Result = "Account created" });
                    }
                }
                else if (request.account_type.ToLower() == "cheque")
                {
                    await _accounts.Add(
                                 new Accounts
                                 {
                                     account_number = request.account_number,
                                     account_type = request.account_type,
                                     balance = request.ammount
                                 });
                    return Ok(new { Success = true, Result = "Account created" });
                }
                return BadRequest(new { Success = false, Result = "Account creation failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("list")]
        public async Task<ActionResult<IEnumerable<Accounts>>> Accounts()
        {
            var accounts = _accounts.GetAll();
            if (accounts == null)
            {
                return NotFound();
            }
            return await accounts.ToListAsync();
        }

        [HttpPost]
        [Route("bank-deposit")]
        public async Task<ActionResult> BankDeposit([FromBody] Request request)
        {
            try
            { 
                if (request.account_number != null && request.ammount.ToString() != null)
                {
                     //&& a.account_type.ToLower() == request.account_type.ToLower()
                    var list = _accounts.GetAll();
                    var account = list.Where(a => a.account_number == request.account_number).FirstOrDefault();       
                    //var account = result.FirstOrDefault();
                    account.balance = account.balance + request.ammount;
                    await _accounts.Update(account);
                    var trans = new Transactions
                    {
                        account_number = request.account_number,
                        account_type = request.account_type,
                        balanceBefore = account.balance - request.ammount,
                        balanceAfter = account.balance,
                        DateTime = DateTime.Now,
                        transactionType = request.account_type,
                        status = "success"
                    };
                   await  _transactions.Add(trans);
                    _accounts.SaveChanges();
                    return Ok(new { Success = true, Result = "Success" });
                }
                return BadRequest(new { Success = false, Result = "Failed to deposit" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = ex.Message
                });
            }

        }

        [HttpPost]
        [Route("bank-withdrawal")]
        public async Task<ActionResult> BankWithdrawal([FromBody] Request request)
        {

            decimal overdraft = 100000;
            try
            {
                if (request.account_number != null && request.ammount.ToString() != null)
                {
                    var list =  _accounts.GetAll();
                    var result = list.Where(a => a.account_number == request.account_number);
                    if (result.FirstOrDefault().account_type.ToLower() == "savings")
                    {
                        if (result.FirstOrDefault().balance < request.ammount)
                        {
                      await  _transactions.Add(
                            new Transactions
                            {
                           account_number = request.account_number,
                           account_type = request.account_type,
                                DateTime = DateTime.Now,
                           transactionType = request.account_type,
                           status = "failed cannot withdraw more than the balance"
                           });
                            return BadRequest(new { Success = false, Result = "Cannot withdraw more than the balance" });
                        }
                    }
                    else if (result.FirstOrDefault().account_type.ToLower() == "cheque")
                    {
                        if (overdraft + result.FirstOrDefault().balance < request.ammount)
                        {
                           await _transactions.Add(
                            new Transactions
                            {
                                account_number = request.account_number,
                                account_type = request.account_type,
                                DateTime = DateTime.Now,
                                transactionType = request.account_type,
                                status = "failed overdraft exceeded R500 limit"
                            });
                            return BadRequest(new { Success = false, Result = "Overdraft exceeded 100 000 limit" });
                        }
                    }
                    var account = result.FirstOrDefault();
                    account.balance = account.balance - request.ammount;
                    await _accounts.Update(account);

                    await _transactions.Add(
                             new Transactions
                             {
                                 account_number = request.account_number,
                                 account_type = request.account_type,
                                 balanceBefore = account.balance + request.ammount,
                                 balanceAfter  = account.balance,
                                 DateTime = DateTime.Now,
                                 transactionType = request.account_type,
                                 status = "success"
                             });         
                }
                _accounts.SaveChanges();
                return Ok(new { Success = true, Result = "Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = ex.Message
                });
            }

        }

        [HttpPost]
        [Route("transactionlist")]
        public async Task<ActionResult<IEnumerable<Transactions>>> Transactions()
        {
            var transactions = _transactions.GetAll();
            if (transactions == null)
            {
                return NotFound();
            }
            return await transactions.ToListAsync();
        }
    }
}


