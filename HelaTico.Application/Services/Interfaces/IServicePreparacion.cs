using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;

namespace HelaTico.Application.Services.Interfaces
{
    public interface IServicePreparacion
    {
        Task<ICollection<PreparacionDTO>> ListAsync();
        Task<PreparacionDTO> FindByIdAsync(int idProducto);
    }
}
