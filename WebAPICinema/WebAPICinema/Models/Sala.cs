using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICinema.Models
{
    public class Sala
    {
        public int IdSala { get; set; }

        public int IdFilme { get; set; }

        public string NomeSala { get; set; }

        public DateTime HorarioSecao { get; set; }

        public int QuantidadeLugares { get; set; }

    }
}
