using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteAcesso.Models;

namespace TesteAcesso.Controllers
{
    [Route("api/consultar")]
    [ApiController]
    public class ConsultarController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ConsultarController(ApiDbContext context)
        {
            _context = context;
        }

        // POST: api/consultar
        [HttpPost]
        public async Task<ActionResult<Dictionary<string, string>>> PostConsultar(Consulta item)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();

            try
            {
                Transaction transaction = await _context.Transactions.FindAsync(item.transactionId);

                if (transaction.Status.CompareTo("Error") == 0)
                {
                    response.Add("Status", transaction.Status);
                    response.Add("Message", transaction.Message);
                }
                else
                {
                    response.Add("Status", transaction.Status);
                }
            }
            catch (Exception)
            {
                response.Add("Status", "Error");
                response.Add("Message", "TransactionId não encontrado.");
            }
            
            return response;
        }
    }
}