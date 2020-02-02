using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TesteAcesso.Models
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; }

        public string AccountOrigin { get; set; }
        
        public string AccountDestination { get; set; }
        
        public float Value { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }

    }
}
