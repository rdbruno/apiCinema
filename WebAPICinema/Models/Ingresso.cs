using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICinema.Models
{
    public class Ingresso
    {
        public int IdIngresso { get; set; }
        public int IdIngressoUsuario { get; set; }
        public int IdUsuario { get; set; }
        public int IdSala { get; set; }
        public int Lugar { get; set; }
        public int IdPagamentoUsuario { get; set; }

        public string RgCpf { get; set; }

        public decimal ValorTotal { get; set; }


    }
}
