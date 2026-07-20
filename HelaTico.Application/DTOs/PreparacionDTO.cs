using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HelaTico.Application.DTOs
{
    public record PreparacionDTO
    {
        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
        [Display(Name = "Producto")]
        public int IdProducto { get; set; }

        [ValidateNever]
        public string NombreProducto { get; set; }

        [ValidateNever]
        public int CantidadPasos { get; set; }

        [ValidateNever]
        public List<PasoPreparacionDTO> Pasos { get; set; } = new();
    }
}
