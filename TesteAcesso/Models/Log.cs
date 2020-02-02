using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TesteAcesso.Models
{
    public class Log
    {
        [Key]
        public string Id { get; set; }
        
        public DateTime Date { get; set; }
        
        public string TransactionId { get; set; }
        
        public string Type { get; set; }
    }
}
