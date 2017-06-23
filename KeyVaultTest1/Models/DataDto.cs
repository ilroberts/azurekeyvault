using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace KeyVaultTest1.Models
{
    public class DataDto
    { 
        [Required, MinLength(3)]
        public string data { get; set; }
    }
}