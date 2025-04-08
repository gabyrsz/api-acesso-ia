using api_acesso_ia.Models;

namespace api_acesso_ia.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        
        Task<IList<Usuario>> ListarTodos();

        
        Task<Usuario> BuscarPorId(int id);

        
        Task<Usuario> Criar(Usuario dados);

        
        Task<bool> Atualizar(Usuario dados);

       
        Task<bool> Deletar(int id);

        

        Task<bool> CpfJaCadastrado(string cpf);

    }
}
