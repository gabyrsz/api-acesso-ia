using api_acesso_ia.Models;
using api_acesso_ia.Services;
using api_acesso_ia.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_acesso_ia.Controllers
{
    [Route("api/v1/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
            public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("listar-todos")]

        public async Task<ActionResult<IList<Usuario>>> ListarTodos()
        {
            var usuarios = await _usuarioService.ListarTodosService();
            return Ok(usuarios);
        }

        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<Usuario>> Buscar(int id)
        {
            var usuario = await _usuarioService.BuscarPorIdService(id);
            if(usuario == null)
            {
                return NotFound(new { msg = "Usuário não encontrado"});
            }
            return Ok(usuario);
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<Usuario>> Salvar([FromBody] Usuario dados)
        {
            if(await _usuarioService.CpfJaCadastradoService(dados.Cpf))
            {
                throw new Exception("O CPF informado já possui cadastro.");
            }
            var usuario = await _usuarioService.CriarService(dados);
            return CreatedAtAction(nameof(Salvar), new { id = dados.Id }, dados);
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> Atualizar(int id,
                                                  [FromBody] Usuario dados)
        {
            if (id != dados.Id)
                return BadRequest(new { msg = "Dados inválidos" });

            await _usuarioService.AtualizarService(dados);
            return NoContent();
        }

        [HttpDelete("remover/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var deletado = await _usuarioService.DeletarService(id);
            if (!deletado)
            {
                return NotFound(new { msg = "Erro ao excluir esse usuário" });
            }
            return NoContent();
        }
    }
}

