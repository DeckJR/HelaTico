using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HelaTico.Application.DTOs
{
    public record EstacionDTO
    {
        [ValidateNever]
        public int IdEstacion { get; set; }

        [ValidateNever]
        public string Descripcion { get; set; }
    }
}