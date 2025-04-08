using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_acesso_ia.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Usuario>> ListarTodos()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> BuscarPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> Criar(Usuario dados)
        {
            _context.Usuarios.Add(dados);
            await _context.SaveChangesAsync();
            return dados;
        }
        public async Task<bool> Atualizar(Usuario dados)
        {
            var usuarioExists = await _context.Usuarios
                                                .FindAsync(dados.Id);
            if (usuarioExists == null)
            {
                return false;
            }

            _context.Entry(usuarioExists).CurrentValues.SetValues(dados);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var dado = await _context.Usuarios.FindAsync(id);

            if (dado == null) return false;

            _context.Usuarios.Remove(dado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CpfJaCadastrado(string cpf)
        {
            return await _context.Usuarios.AnyAsync(u => u.Cpf == cpf);
        }
    }
}
