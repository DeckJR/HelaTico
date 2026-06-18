using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public record ComboDTO
    {
        public int IdCombo { get; init; }
        public string Nombre { get; init; }
        public string Descripcion { get; init; }
        public decimal Precio { get; init; }
        public byte[] Imagen { get; set; }

    }
}
