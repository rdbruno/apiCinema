using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICinema.Models
{
    public class Pagamento
    {
        public int IdPagamento { get; set; }
        public int IdPagamentoUsuario { get; set; }
        public int IdFormaPagamento { get; set; }
        public Int64 NumeroCartao { get; set; }

        public DateTime DataExpiracao { get; set; }

        public int CodigoSeguranca { get; set; }

    }
}
