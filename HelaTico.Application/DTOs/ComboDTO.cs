using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public record ComboDTO
    {
        public int IdCombo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public byte[] Imagen { get; set; }
        public string EstadoCombo { get; init; }
        public int CantidadProductos { get; set; }
        public List<ComboProductoDTO> Productos { get; set; } = new();
    }
}
