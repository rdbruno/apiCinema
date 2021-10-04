using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICinema.Models
{
    public class Filmes
    {
        public int IdFilme { get; set; }

        public string NomeFilme { get; set; }

        public decimal PrecoIngresso { get; set; }
    }
}
