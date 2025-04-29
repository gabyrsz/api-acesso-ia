
using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using api_acesso_ia.Models;
using api_acesso_ia.Responses;
using api_acesso_ia.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace api_acesso_ia.Services
{
    public class ImagemService : IImagemService
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly RegionEndpoint region = RegionEndpoint.USEast1;

        public ImagemService(IOptions<AwsSettings> awsSettings)
        {
            _accessKey = awsSettings.Value.AccessKey;
            _secretKey = awsSettings.Value.SecretKey;
        }
        

        public async Task<ImagemResponse> CompararImagensAsync(string imagemBase64_1, string imagemBase64_2)
        {
            try
            {
                string base64_1 = LimparBase64(imagemBase64_1);
                string base64_2 = LimparBase64(imagemBase64_2);

                using var sourceStream = new MemoryStream(Convert.FromBase64String(base64_1));
                using var targetStream = new MemoryStream(Convert.FromBase64String(base64_2));

                var rekognitionClient = new AmazonRekognitionClient(_accessKey, _secretKey, region);

                var request = new CompareFacesRequest
                {
                    SourceImage = new Image { Bytes = sourceStream },
                    TargetImage = new Image { Bytes = targetStream },
                    SimilarityThreshold = 70F
                };

                var response = await rekognitionClient.CompareFacesAsync(request);

                if (response.FaceMatches.Count > 0)
                {
                    var similaridade = response.FaceMatches[0].Similarity;
                    return new ImagemResponse
                    {
                        Similaridade = similaridade,
                        Iguais = similaridade >= 70
                    };
                }

                return new ImagemResponse { Similaridade = 0, Iguais = false };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro AWS Rekognition: {ex.Message}");
                return new ImagemResponse { Similaridade = 0, Iguais = false };
            }
        }

        private string LimparBase64(string base64)
        {
            if (base64.Contains(","))
                base64 = base64[(base64.IndexOf(",") + 1)..];

            return base64.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
        }

    }
}