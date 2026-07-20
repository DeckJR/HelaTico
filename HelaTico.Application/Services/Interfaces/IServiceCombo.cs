using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelaTico.Application.DTOs;

namespace HelaTico.Application.Services.Interfaces
{
    public interface IServiceCombo
    {
        Task<ICollection<ComboDTO>> ListAsync();
        Task<ComboDTO> FindByIdAsync(int id);
        Task AddAsync(ComboDTO dto, int[] productosIds, int[] cantidades);
        Task UpdateAsync(int id, ComboDTO dto, int[] productosIds, int[] cantidades);
        Task DeleteAsync(int id);
    }
}
