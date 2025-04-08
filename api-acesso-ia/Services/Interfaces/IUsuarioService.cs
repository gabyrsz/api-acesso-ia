using api_acesso_ia.Models;

namespace api_acesso_ia.Services.Interfaces
{
    public interface IUsuarioService
    {
        
        Task<IList<Usuario>> ListarTodosService();

        
        Task<Usuario> BuscarPorIdService(int id);

        
        Task<Usuario> CriarService(Usuario dados);

        
        Task<bool> AtualizarService(Usuario dados);

        Task<bool> DeletarService(int id);

        Task<bool> CpfJaCadastradoService(string cpf);
    }
}
