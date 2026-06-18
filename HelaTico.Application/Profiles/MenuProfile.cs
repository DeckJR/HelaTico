using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HelaTico.Infraestructure.Models;
using HelaTico.Application.DTOs;
using HelaTico.Application.Enums;


namespace HelaTico.Application.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {

            CreateMap<Menu, MenuDTO>()

                           .ForMember(dest => dest.EstadoMenu,
                               opt => opt.MapFrom(src =>
                                   ((EstadoMenu)src.EstadoMenu)
                                   .ToString()
                                   .Replace("_", " ")
                               ))


                           .ForMember(dest => dest.Productos,
                             opt => opt.MapFrom(src =>
                            src.IdProducto != null
                            ? src.IdProducto
                            : new List<Producto>()
                              ))


                            .ForMember(dest => dest.Combos,
                                opt => opt.MapFrom(src =>
                                    src.IdCombo != null
                                        ? src.IdCombo
                                        : new List<Combo>()
                                ));

        }
    }

}

