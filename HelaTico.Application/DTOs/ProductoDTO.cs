using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HelaTico.Application.DTOs
{
    public record ProductoDTO
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} debe tener entre {2} y {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(250, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es un dato requerido")]
        [Range(0.01, 9999999, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [ValidateNever]
        public string Categoria { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "Debe seleccionar una {0}")]
        public int IdCategoria{ get; set;}

        [Required(ErrorMessage = "Debe seleccionar al menos un ingrediente")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un ingrediente")]
        public int[] IdIngrediente { get; set; } = Array.Empty<int>();

        [ValidateNever]
        public List<string> Ingredientes { get; set; } = new();

        [ValidateNever]
        public byte[] Imagen { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Debe seleccionar un {0}")]
        public int EstadoProductoId { get; set; }

        [ValidateNever]
        public string EstadoProducto { get; init; }
    }
}
