using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_acesso_ia.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Status { get; set; }

        [Column("foto_perfil")]
        public string? foto_perfil { get; set; }
        public string? FotoPerfilBase64Resumido
        => string.IsNullOrEmpty(foto_perfil) ||
            foto_perfil.Length <= 30 ? foto_perfil :
            foto_perfil.Substring(0, 30) + "...";
    }
}