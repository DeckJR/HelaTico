using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public class CarritoItemDTO
    {
        public string Tipo { get; set; } = null!; // "Producto" o "Combo"
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string ImagenUrl { get; set; } = null!;
        public string Observaciones { get; set; } = string.Empty;
    }
}