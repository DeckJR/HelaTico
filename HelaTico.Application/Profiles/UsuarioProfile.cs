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
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.DescripcionRol,
                    opt => opt.MapFrom(src => src.IdRolUsuarioNavigation.Descripcion))
                .ReverseMap()
                .ForMember(dest => dest.IdRolUsuarioNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.PedidoIdClienteNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.PedidoIdEmpleadoNavigation, opt => opt.Ignore());
        }
    }
}