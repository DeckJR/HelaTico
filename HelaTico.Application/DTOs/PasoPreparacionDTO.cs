using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelaTico.Application.DTOs
{
    //Clase para poder manejar la línea donde se muestre la estación y el orden de la preparación, para luego mostrarlo en la vista del proceso de preparacion
    public record PasoPreparacionDTO
    {
        public string NombreEstacion { get; set; }
        public int Orden { get; set; }
    }
}
