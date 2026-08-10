using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Infraestructure.Models
{
    public class TipoCambio
    {
        public decimal Compra { get; set; }
        public decimal Venta { get; set; }
        public string Fecha { get; set; } = string.Empty;
    }
}
