using api_acesso_ia.Models;

namespace api_acesso_ia.Services.Interfaces
{
    public interface ILoginService
    {
        
        Task<IList<Login>> ListarTodosService();

        
        Task<Login> CriarService(Login dados);


        Task<Login> AutenticarService(string login, string senha);


        Task<Login> CadastrarService(Login dados);


        Task<bool> CpfCadastradoService(string cpf);


        string Criptografar(string senha);


        Task<Login> BuscarEmailService(string email);


        Task<bool> RedefinirSenhaService(int idUsuario, string SenhaNova);


    }
}
 