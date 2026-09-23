using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data{
    public class BancoContext : DbContext{
        public BancoContext(DbContextOptions<BancoContext> options) : base(options){
        }
        public DbSet<CadastroModel> Cadastro {get; set;}

        public DbSet<PerfilModel> Perfil {get; set;}

        public DbSet<PerfilUsuario> PerfilUsuario {get; set;}

        //public DbSet<LoginModel> Login{get; set;}
        
    }
}