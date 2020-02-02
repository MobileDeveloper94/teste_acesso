using Microsoft.EntityFrameworkCore;

namespace TesteAcesso.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

       
    }
}
