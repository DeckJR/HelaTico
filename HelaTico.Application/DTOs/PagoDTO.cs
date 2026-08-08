using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace HelaTico.Application.DTOs
{
    public class PagoDTO
    {
        public int IdPago { get; set; }
        public string MetodoPagoTexto { get; set; } = null!;
        public decimal Monto { get; set; }
        public decimal Vuelto { get; set; }
        public DateTime Fecha { get; set; }
    }
}
