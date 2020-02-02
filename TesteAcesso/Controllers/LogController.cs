using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteAcesso.Models;


namespace TesteAcesso.Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public LogController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/log
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> GetLog()
        {
            return await _context.Logs.ToListAsync();
        }
    }
}