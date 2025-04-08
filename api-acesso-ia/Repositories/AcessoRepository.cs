using api_acesso_ia.Models;
using api_acesso_ia.Dtos;
using api_acesso_ia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_acesso_ia.Repositories
{
    public class AcessoRepository : IAcessoRepository
    {
        private readonly AppDbContext _context;

        public AcessoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AcessoResponse>> ListarTodos()
        {
            return await _context.Acessos
                .Include(a => a.Usuario) 
                .OrderByDescending(a => a.DataHoraAcesso)
                .Select(a => new AcessoResponse
                {
                    Id = a.Id,
                    IdUsuario = a.IdUsuario,
                    NomeUsuario = a.Usuario.Nome, 
                    DataHoraAcesso = a.DataHoraAcesso
                })
                .ToListAsync();
        }


        public async Task<bool> Registrar(Acesso acesso)
        {
            _context.Acessos.Add(acesso);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}