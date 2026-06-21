using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public record ComboProductoDTO
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
    }
}
