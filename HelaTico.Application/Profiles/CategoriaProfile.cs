using AutoMapper;
using HelaTico.Infraestructure.Models;
using HelaTico.Application.DTOs;

namespace HelaTico.Application.Profiles
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<Categoria, CategoriaDTO>();
        }
    }
}