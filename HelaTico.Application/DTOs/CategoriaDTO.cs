using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public record CategoriaDTO
    {
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}