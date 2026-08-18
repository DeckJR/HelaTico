using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    public class EstacionProcesoDTO
    {
        public int IdEstacion { get; set; }
        public string Descripcion { get; set; }= string.Empty;
        public int OrdenesPendientes { get; set; }
        public int OrdenesEnProceso { get; set; }
    }
}