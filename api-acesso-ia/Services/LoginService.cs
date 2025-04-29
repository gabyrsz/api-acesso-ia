    using api_acesso_ia.Models;
    using api_acesso_ia.Repositories;
    using api_acesso_ia.Repositories.Interfaces;
    using api_acesso_ia.Services.Interfaces;
    using MailKit.Security;
    using MimeKit;
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

            public async Task<bool> RedefinirSenhaService(int idUsuario, string SenhaNova)
            {
                var usuario = await _loginRepository.BuscarId(idUsuario);
                if (usuario == null) return false;

                usuario.Senha = Criptografar(SenhaNova);

                await _loginRepository.Atualizar(usuario);

                await this.EnviarEmailAsync(usuario.Email,
                       "Sua senha foi resetada",
                       $"Olá {usuario.Nome}, sua senha foi redefinida com sucesso!");

                return true;
            }

            public async Task<bool> EnviarEmailAsync(string destinatario, string assunto, string mensagem)
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Meu sistema", "gabriellerossi.ifmg@gmail.com"));
                email.To.Add(MailboxAddress.Parse(destinatario));
                email.Subject = assunto;

                email.Body = new TextPart("plain") { Text = mensagem };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                try
                {
                    await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync("gabriellerossi.ifmg@gmail.com", "okxm irce ubfo hekd"); // App Password
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar email: {ex.Message}");   
                    return false;
                }
            }
        }
    }


