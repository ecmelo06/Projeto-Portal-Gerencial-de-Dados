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
            public PerfilModel Buscar(PerfilModel perfil){
                return _bancoContext.Perfil.FirstOrDefault(x => x.Nome == perfil.Nome);
            }

        }


}