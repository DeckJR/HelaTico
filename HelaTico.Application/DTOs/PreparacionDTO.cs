using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace HelaTico.Application.DTOs
{
    public record PreparacionDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadPasos { get; set; }
        public List<PasoPreparacionDTO> Pasos { get; set; } = new();
    }
}
