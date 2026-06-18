using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public record MenuDTO
    {
        public int IdMenu { get; init; }
        public string Nombre { get; init; }

        public DateTime FechaInicio { get; init; }
        public DateTime FechaFinal { get; init; }

        public string EstadoMenu { get; init; }
        public List<ProductoDTO> Productos { get; init; }
        public List<ComboDTO> Combos { get; init; } = new();
    }
}

