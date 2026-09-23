using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class CadastroModel{
        public int Id {get; set;}
        public string Nome {get; set;}
        public string Senha {get; set;}
        public string Email {get; set;}
        public string CPF {get; set;}
        public string Orgao {get; set;}
        [NotMapped]
        public string Perfil { get; set; }

    }
}