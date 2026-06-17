using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public record ProductoDTO
    {
        public int IdProducto { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }

        public string Categoria { get; set; }

        public List<string> Ingredientes { get; set; } = new();

        public byte[] Imagen { get; set; }

    }
}
