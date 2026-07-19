using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HelaTico.Application.DTOs
{
    public record MenuDTO
    {
        public int IdMenu { get; init; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(50, ErrorMessage = "{0} no puede superar los {1} caracteres")]
        public string Nombre { get; init; }

        [Required(ErrorMessage = "La fecha de inicio es un dato requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; init; }

        [Required(ErrorMessage = "La fecha final es un dato requerido")]
        [DataType(DataType.Date)]
        public DateTime FechaFinal { get; init; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Debe seleccionar un {0}")]
        public int EstadoMenuId { get; set; }

        [ValidateNever]
        public string EstadoMenu { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos un producto")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un producto")]
        public int[] IdProducto { get; set; } = Array.Empty<int>();
        
        [ValidateNever]
        public List<ProductoDTO> Productos { get; set; } = new();

        public int[] IdCombo { get; set; } = Array.Empty<int>();

        [ValidateNever]
        public List<ComboDTO> Combos { get; set; } = new();
    }
}

