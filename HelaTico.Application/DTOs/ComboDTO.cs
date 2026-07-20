using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HelaTico.Application.DTOs
{
    public record ComboDTO
    {
        [Display(Name = "Identificador Combo")]
        [ValidateNever]
        public int IdCombo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} debe tener entre 2 y 50 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es un dato requerido.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} debe tener entre 5 y 50 caracteres.")]
        public string Descripcion { get; set; }

        [Display(Name = "Precio (₡)")]
        [Required(ErrorMessage = "{0} es un dato requerido.")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a ₡0.")]
        public decimal Precio { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} es un dato requerido.")]
        public int Estado { get; set; } = 1;

        [Display(Name = "Imagen")]
        [ValidateNever]
        public byte[] Imagen { get; set; }

        [ValidateNever]
        public string EstadoCombo { get; init; }

        [ValidateNever]
        public int CantidadProductos { get; set; }

        [ValidateNever]
        public List<ComboProductoDTO> Productos { get; set; } = new();
    }
}
