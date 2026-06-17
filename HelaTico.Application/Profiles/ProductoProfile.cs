using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HelaTico.Application.DTOs;
using HelaTico.Infraestructure.Models;

namespace HelaTico.Application.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile() {
            CreateMap<Producto, ProductoDTO>()

        // Categoría
        .ForMember(dest => dest.Categoria,
            opt => opt.MapFrom(src => src.IdCategoriaNavigation.Descripcion))

        // Ingredientes
        .ForMember(dest => dest.Ingredientes,
            opt => opt.MapFrom(src =>
                src.IdIngrediente != null
                    ? src.IdIngrediente.Select(i => i.Descripcion)
                    : new List<string>()
            ));
        }


    }
}
