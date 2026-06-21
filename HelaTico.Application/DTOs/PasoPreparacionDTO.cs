using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public record PasoPreparacionDTO
    {
        public string NombreEstacion { get; set; }
        public int Orden { get; set; }
    }
}
