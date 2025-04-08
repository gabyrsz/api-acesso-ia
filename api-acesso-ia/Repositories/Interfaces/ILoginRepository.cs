using api_acesso_ia.Models;

namespace api_acesso_ia.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        
        Task<IList<Login>> ListarTodos();
 
       
        Task<Login> Criar(Login dados);

        Task<Login> Autenticar(string login, string senha);


        Task<Login> Cadastrar(Login dados);


        Task<bool> CpfCadastrado(string cpf);


        Task<Login> BuscarEmail(string email);


        Task<Login> BuscarId(int id);

        Task Atualizar(Login dados);

    }
} 
