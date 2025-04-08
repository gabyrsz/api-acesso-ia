using api_acesso_ia.Models;
using api_acesso_ia.Repositories;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;


namespace api_acesso_ia.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
             _loginRepository = loginRepository;
        }

        public async Task<IList<Login>> ListarTodosService()
        { 
           return await _loginRepository.ListarTodos();
        }
        public async Task<Login> CriarService(Login dados)
        {
           return await _loginRepository.Criar(dados);
        }

        public async Task<Login> AutenticarService(string login, string senha)
        {

            var senhaHash = Criptografar(senha);
            return await _loginRepository.Autenticar(login, senhaHash);
        }

        public async Task<Login> CadastrarService(Login dados)
        {
            return await _loginRepository.Cadastrar(dados);
        }

        public async Task<bool> CpfCadastradoService(string cpf)
        {
            var cadastrado = await _loginRepository.CpfCadastrado(cpf);

            if (cadastrado)
            {
                return true;
            }
            return false;
        }

        public string Criptografar(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<Login> BuscarEmailService(string email)
        {
            return await _loginRepository.BuscarEmail(email);
        }

        public async Task<bool> RedefinirSenhaService(int idUsuario)
        {
            var usuario = await _loginRepository.BuscarId(idUsuario);
            if (usuario == null) return false;

            var redefinida = usuario.Cpf;
            usuario.Senha = Criptografar(redefinida);

            await _loginRepository.Atualizar(usuario);

            return true;
        }


    }
}


