using api_acesso_ia.Models;
using api_acesso_ia.Request;
using api_acesso_ia.Services;
using api_acesso_ia.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_acesso_ia.Controllers
{
    [Route("api/v1/logins")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
            public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        } 

        [HttpGet("listar-todos")]

        public async Task<ActionResult<IList<Login>>> ListarTodos()
        {
            var logins = await _loginService.ListarTodosService();
            return Ok(logins);
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<Login>> Salvar([FromBody] Login dados)
        {
            if (await _loginService.CpfCadastradoService(dados.Cpf))
            {
                throw new Exception("Esse CPF já tem cadastro.");
            }

            dados.Senha = _loginService.Criptografar(dados.Senha);

            var usuario = await _loginService.CadastrarService(dados);
            return CreatedAtAction(nameof(Salvar), new { id = dados.Id }, dados);
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult<Login>> Autenticar([FromBody] LoginRequest dados)
        {
            var usuario = await _loginService.AutenticarService(dados.login, dados.senha);

            if (usuario == null)
            {
                return Unauthorized(new { msg = "Login ou senha inválidos!" });
            }

            return Ok(new {Id = usuario.Id, Nome = usuario.Nome, Email = usuario.Email, Cpf = usuario.Cpf });
        }


        [HttpGet("buscar-email")]
        public async Task<IActionResult> BuscarEmail([FromQuery] string email)
        {
            var usuario = await _loginService.BuscarEmailService(email);
            if (usuario == null)
                return NotFound("O usuário não foi encontrado");

            return Ok(usuario);
        }

        [HttpPut("redefinir-senha/{idUsuario}")]
        public async Task<IActionResult> RedefinirSenha(int idUsuario, [FromBody] RedefinirSenhaRequest dados)
        {
            var redefinida = await _loginService.RedefinirSenhaService(idUsuario, dados.SenhaNova);

            if (!redefinida)
                return NotFound("O usuário não foi encontrado");

            return Ok("Senha redefinida!");
        }

    }
}


