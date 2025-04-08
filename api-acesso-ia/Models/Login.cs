using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_acesso_ia.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string Login_usuario { get; set; }
        public string Senha { get; set; } 
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }


    }
}
