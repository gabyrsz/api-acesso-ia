using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_acesso_ia.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext _context;

        public LoginRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Login>> ListarTodos()
        { 
            return await _context.Logins.ToListAsync();
        }

        public async Task<Login> Criar(Login dados)
        {
            _context.Logins.Add(dados);
            await _context.SaveChangesAsync();
            return dados;
        }
        public async Task<Login> Autenticar(string login, string senha)
        {
            return await _context.Logins.Where(l => l.Login_usuario == login && l.Senha == senha).FirstOrDefaultAsync();
        }

        public async Task<Login> Cadastrar(Login dados)
        {
            _context.Logins.Add(dados);
            await _context.SaveChangesAsync();
            return dados;
        }
        public async Task<bool> CpfCadastrado(string cpf)
        {
            return await _context.Logins.AnyAsync(u => u.Cpf == cpf);
        }

        public async Task<Login> BuscarEmail(string email)
        {
            return await _context.Logins.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Login> BuscarId(int id)
        {
            return await _context.Logins.FindAsync(id);
        }

        public async Task Atualizar(Login dados)
        {
            _context.Logins.Update(dados);
            await _context.SaveChangesAsync();
        }

    }
}
