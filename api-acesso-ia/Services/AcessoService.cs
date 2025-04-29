using api_acesso_ia.Models;
using api_acesso_ia.Dtos;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Services.Interfaces;
using MailKit.Security;
using MimeKit;

namespace api_acesso_ia.Services
{
    public class AcessoService : IAcessoService
    {
        private readonly IAcessoRepository _acessoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public AcessoService(IAcessoRepository acessoRepository, IUsuarioRepository usuarioRepository)
        {
            _acessoRepository = acessoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public Task<IEnumerable<AcessoResponse>> ListarTodos()
        {
            return _acessoRepository.ListarTodos();
        }

        public async Task<bool> Registrar(Acesso acesso)
        {
            var registrado = await _acessoRepository.Registrar(acesso);

            if (!registrado)
                return false;

            // Buscar dados do usuário
            var usuario = await _usuarioRepository.BuscarPorId(acesso.IdUsuario);

            if (usuario != null)
            {
                    var horarioBrasilia = TimeZoneInfo.ConvertTimeFromUtc(acesso.DataHoraAcesso.ToUniversalTime(),
        TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

                    var data = horarioBrasilia.ToString("dd/MM/yyyy HH:mm");

                var assunto = "Confirmação de Acesso";
                var mensagem = $"{usuario.Nome}, seu registro de acesso foi cadastrado em {data}.";

                await EnviarEmailAcesso(usuario.Email, assunto, mensagem);
            }

            return true;
        }

        private async Task<bool> EnviarEmailAcesso(string destinatario, string assunto, string mensagem)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Sistema de Acessos", "gabriellerossi.ifmg@gmail.com"));
            email.To.Add(MailboxAddress.Parse(destinatario));
            email.Subject = assunto;
            email.Body = new TextPart("plain") { Text = mensagem };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("gabriellerossi.ifmg@gmail.com", "okxm irce ubfo hekd");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                return false;
            }
        }
    }
}
