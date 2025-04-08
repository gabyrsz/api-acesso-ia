using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Services.Interfaces;

namespace api_acesso_ia.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
             _usuarioRepository = usuarioRepository;
        }

        public async Task<IList<Usuario>> ListarTodosService()
        {
           return await _usuarioRepository.ListarTodos();
        }
        public async Task<Usuario> BuscarPorIdService(int id)
        {
           return await _usuarioRepository.BuscarPorId(id);
        }
        public async Task<Usuario> CriarService(Usuario dados)
        {
           return await _usuarioRepository.Criar(dados);
        }
        public async Task<bool> AtualizarService(Usuario dados)
        {
            return await _usuarioRepository.Atualizar(dados);
        }

        public async Task<bool> DeletarService(int id)
        {
            return await _usuarioRepository.Deletar(id);
        }


        public async Task<bool> CpfJaCadastradoService(string cpf)
        {
           var possui =  await _usuarioRepository.CpfJaCadastrado(cpf);
            if (possui)
            {
                return true;
            }
            return false;
        }
    }

}
    