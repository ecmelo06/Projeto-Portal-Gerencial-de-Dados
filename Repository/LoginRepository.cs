using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Project.Data;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Project.Controllers;

namespace Project.Repository
{
        public class LoginRepository : ILoginRepository{
            private readonly BancoContext _bancoContext;
            public LoginRepository(BancoContext bancoContext)
            {
                _bancoContext = bancoContext;
            }
            public CadastroModel Selecionar(LoginModel login){
            
            CadastroModel usuario = _bancoContext.Cadastro.FirstOrDefault(x => x.Email == login.Email);

            if (usuario == null)
            {
                return null;
            }
               usuario.Perfils = _bancoContext.PerfilUsuario
                            .Where(pu => pu.CadastroId == usuario.Id)
                            .Join(_bancoContext.Perfil, 
                                pu => pu.PerfilId,      
                                p => p.Id,             
                                (pu, p) => new PerfilModel 
                                {
                                    Nome = p.Nome
                                })
                            .ToList();
                return usuario;
            }
            public List<CadastroModel> BuscarTodos(){
                var cadastros = _bancoContext.Cadastro.ToList(); 

                foreach (var usuario in cadastros)
                {
                    usuario.Perfils = _bancoContext.PerfilUsuario
                        .Where(pu => pu.CadastroId == usuario.Id)
                        .Join(_bancoContext.Perfil, 
                            pu => pu.PerfilId,      
                            p => p.Id,             
                            (pu, p) => new PerfilModel 
                            {
                                Nome = p.Nome
                            })
                        .ToList();
                }
                return cadastros; 
            }
            public PerfilModel Buscar(PerfilModel perfil){
                return _bancoContext.Perfil.FirstOrDefault(x => x.Nome == perfil.Nome);
            }

        public CadastroModel BuscarPorId(int id){
            var usuario = _bancoContext.Cadastro.FirstOrDefault(x => x.Id == id);
            if (usuario != null)
            {
                usuario.Perfils = _bancoContext.PerfilUsuario
                    .Where(pu => pu.CadastroId == usuario.Id)
                    .Join(_bancoContext.Perfil, pu => pu.PerfilId, p => p.Id, (pu, p) => new PerfilModel { Nome = p.Nome })
                    .ToList();
            }
            return usuario;
        }
        public bool VerificaEmail(string email, int idAtual){
            return _bancoContext.Cadastro.Any(x => x.Email == email && x.Id != idAtual);
        }
        public bool Atualizar(CadastroModel usuario){
            if (usuario == null || usuario.Id <= 0) return false;

            try{
            var entry = _bancoContext.Cadastro.Attach(usuario);
            entry.State = EntityState.Modified;
            entry.Property(x => x.Senha).IsModified = false;

            _bancoContext.SaveChanges();
            return true;
            }
            catch (Exception ex){
            Console.WriteLine($"Erro ao atualizar no EF: {ex.Message}");
            return false;
            }
        }


        public bool PromoverParaAdmin(int usuarioId)
        {
            var perfilAdmin = _bancoContext.Perfil.FirstOrDefault(x => x.Nome == "Admin");
            if (perfilAdmin == null) return false;

            var jaEAdmin = _bancoContext.PerfilUsuario.Any(x => x.CadastroId == usuarioId && x.PerfilId == perfilAdmin.Id);
            if (jaEAdmin) return true;

            var novoPerfil = new PerfilUsuario 
            {
                CadastroId = usuarioId,
                PerfilId = perfilAdmin.Id
            };

            _bancoContext.PerfilUsuario.Add(novoPerfil);
            _bancoContext.SaveChanges();
            return true;
        }


        public bool Excluir(int id)
        {
            var usuario = _bancoContext.Cadastro.FirstOrDefault(x => x.Id == id);
            if (usuario == null) return false;

            var perfisVinculados = _bancoContext.PerfilUsuario.Where(x => x.CadastroId == id);
            _bancoContext.PerfilUsuario.RemoveRange(perfisVinculados);

            _bancoContext.Cadastro.Remove(usuario);
            _bancoContext.SaveChanges();
            return true;
        }
    }


}