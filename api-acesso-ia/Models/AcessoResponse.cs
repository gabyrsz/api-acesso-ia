namespace api_acesso_ia.Dtos
{
    public class AcessoResponse
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataHoraAcesso { get; set; }
    }
}