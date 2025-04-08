using api_acesso_ia.Models;
using api_acesso_ia.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_acesso_ia.Services.Interfaces
{
    public interface IAcessoService
    {
        Task<IEnumerable<AcessoResponse>> ListarTodos();
        Task<bool> Registrar(Acesso acesso);
    }
}