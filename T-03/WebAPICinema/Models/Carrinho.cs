using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebAPICinema.Models
{


    public class Carrinho 
    {
        public int IdCarrinho { get; set; }

        public int IdUsuario { get; set; }

        public int IdSala { get; set; }

        public int Lugar { get; set; }

        public decimal ValorTotal { get; set; }
    }
}
