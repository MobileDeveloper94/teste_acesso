using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteAcesso.Models;
using Flurl.Http;

namespace TesteAcesso.Controllers
{
    [Route("api/transferir")]
    [ApiController]
    public class TransferirController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TransferirController(ApiDbContext context)
        {
            _context = context;
        }

        // POST: api/Transferir
        [HttpPost]
        public async Task<Dictionary<string, string>> PostTransferir(Transaction item)
        {
            //1 - persistir log e transaction
            Transaction transaction = new Transaction();

            transaction.AccountOrigin = item.AccountOrigin;
            transaction.AccountDestination = item.AccountDestination;
            transaction.Value = item.Value;
            transaction.Status = "In Queue";

            _context.Add(transaction);
            _context.SaveChanges();

            Log log = new Log();

            log.Date = DateTime.Now;
            log.TransactionId = transaction.Id;
            log.Type = "Transaction";

            _context.Add(log);
            _context.SaveChanges();

            Dictionary<string, string> response = new Dictionary<string, string>();
            response.Add("transactionId", transaction.Id);

            //2 - subtrair da account 1
            try
            {
                transaction.Status = "Processing";
                _context.Entry(transaction).State = EntityState.Modified;
                _context.SaveChanges();

                var uri = await "https://acessoaccount.herokuapp.com/api/account"
                .WithHeader("Accept", "application/json")
                .PostJsonAsync(new { accountNumber = transaction.AccountOrigin, value = transaction.Value, type = "Debit" });
                
                if (!uri.IsSuccessStatusCode)
                {
                    transaction.Status = "Error";
                    transaction.Message = uri.Content.ToString();
                    _context.Entry(transaction).State = EntityState.Modified;
                    _context.SaveChanges();
                    
                }
                
            }
            catch (Exception ex)
            {
                transaction.Status = "Error";
                transaction.Message = ex.Message;
                _context.Entry(transaction).State = EntityState.Modified;
                _context.SaveChanges();
                
            }

            //3 - adicionar na account 2
            try
            {
                var uri = await "https://acessoaccount.herokuapp.com/api/account"
                .WithHeader("Accept", "application/json")
                .PostJsonAsync(new { accountNumber = transaction.AccountDestination, value = transaction.Value, type = "Credit" });

                if (uri.IsSuccessStatusCode)
                {
                    transaction.Status = "Confirmed";
                    _context.Entry(transaction).State = EntityState.Modified;
                    _context.SaveChanges();
                    
                }
                else
                {
                    transaction.Status = "Error";
                    transaction.Message = uri.Content.ToString();
                    _context.Entry(transaction).State = EntityState.Modified;
                    _context.SaveChanges();
                    
                }
            }
            catch (Exception ex)
            {
                transaction.Status = "Error";
                transaction.Message = ex.Message;
                _context.Entry(transaction).State = EntityState.Modified;
                _context.SaveChanges();
                
            }

            return response;
        }



    }
}
