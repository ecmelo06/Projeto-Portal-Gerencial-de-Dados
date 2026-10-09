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
        
        public DbSet<RecuperacaoSenhaModel> RecuperacaoSenha {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PerfilModel>().HasData(
                new PerfilModel {Id = 1, Nome="Admin"},
                new PerfilModel {Id = 2, Nome="AGERGS"},
                new PerfilModel {Id = 3, Nome="Admin"},
                new PerfilModel {Id = 4, Nome="Externo"},
                new PerfilModel {Id = 5, Nome="SuperAdmin"}
            );
        }
    }
}