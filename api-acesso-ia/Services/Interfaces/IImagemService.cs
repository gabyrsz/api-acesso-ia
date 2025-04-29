using api_acesso_ia.Responses;

namespace api_acesso_ia.Services.Interfaces
{
    public interface IImagemService
    {
        Task<ImagemResponse> CompararImagensAsync(string imagemBase64_1, string imagemBase64_2);
    }
}