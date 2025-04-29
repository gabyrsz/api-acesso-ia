
using api_acesso_ia.Request;
using api_acesso_ia.Responses;
using api_acesso_ia.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api_acesso_ia.Controllers
{
    [Route("api/v1/imagens")]
    [ApiController]
    public class ImagemController : ControllerBase
    {
        private readonly IImagemService _imagemService;

        public ImagemController(IImagemService imagemService)
        {
            _imagemService = imagemService;
        }

        [HttpPost("comparar-imagens")]
        public async Task<ActionResult<ImagemResponse>> CompararImagens([FromBody] ImagemRequest request)
        {
            var resultado = await _imagemService.CompararImagensAsync(request.ImagemBase64_1, request.ImagemBase64_2);
            return Ok(resultado);
        }
    }
}