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
<<<<<<< HEAD
       // public string Perfil { get; set; }
        public List<PerfilModel> Perfils {get; set; }
=======
        public string Perfil { get; set; }
>>>>>>> fc7e423239962f5acc4877ca765feb7a52eab656

    }
}