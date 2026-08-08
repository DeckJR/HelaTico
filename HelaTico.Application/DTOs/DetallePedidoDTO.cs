using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace HelaTico.Application.DTOs
{
    public class DetallePedidoDTO
    {
        public int IdDetallePedido { get; set; }
        public string NombreItem { get; set; } = null!;
        //si es un item o un combo ya que como lo manejamos en la db con tabla
        //que permite ambos nuleables se tiene que identificar cual Tipo de Item es
        //si es un combo o un producto
        public string TipoItem { get; set; } = null!;   
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal TotalLinea { get; set; }
        public string Observaciones { get; set; } = string.Empty;
    }
}