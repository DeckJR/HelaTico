using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public class TipoEntregaDTO
    {
        public int IdTipoEntrega { get; set; }

        public string Descripcion { get; set; } = string.Empty;
    }
}