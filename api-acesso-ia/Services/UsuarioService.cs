using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text;
using System.Threading.Tasks;

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
            var usuario = await _usuarioRepository.Criar(dados);

            // Enviar e-mail de boas-vindas
            if (!string.IsNullOrWhiteSpace(usuario.Email))
            {
                await EnviarEmailAsync(
                    usuario.Email,
                    "Cadastro realizado com sucesso",
                    $"Olá, {usuario.Nome}! Seja muito bem-vindo(a) ao nosso sistema.\n\nSeu cadastro foi realizado com sucesso!"
                );
            }

            return usuario;
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
            var possui = await _usuarioRepository.CpfJaCadastrado(cpf);
            return possui;
        }

        private async Task<bool> EnviarEmailAsync(string destinatario, string assunto, string mensagem)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Meu sistema", "gabriellerossi.ifmg@gmail.com"));
            email.To.Add(MailboxAddress.Parse(destinatario));
            email.Subject = assunto;
            email.Body = new TextPart("plain") { Text = mensagem };

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("gabriellerossi.ifmg@gmail.com", "okxm irce ubfo hekd"); // senha de app
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar email de boas-vindas: {ex.Message}");
                return false;
            }
        }
    }
}
