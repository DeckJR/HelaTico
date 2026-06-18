using AutoMapper;
using HelaTico.Infraestructure.Models;
using HelaTico.Application.DTOs;
using HelaTico.Application.Enums;
using System.Linq;

namespace HelaTico.Application.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Producto, ProductoDTO>()

          
                .ForMember(dest => dest.Categoria,
                    opt => opt.MapFrom(src => src.IdCategoriaNavigation.Descripcion))

                .ForMember(dest => dest.Ingredientes,
                    opt => opt.MapFrom(src =>
                        src.IdIngrediente != null
                            ? src.IdIngrediente.Select(i => i.Descripcion).ToList()
                            : new List<string>()
                    ))

                .ForMember(dest => dest.EstadoProducto,
                    opt => opt.MapFrom(src =>
                        ((EstadoMenu)src.EstadoProducto)
                        .ToString()
                        .Replace("_", " ")
                    ))

                .ForMember(dest => dest.Imagen,
                    opt => opt.MapFrom(src => src.Imagen));
        }
    }
}