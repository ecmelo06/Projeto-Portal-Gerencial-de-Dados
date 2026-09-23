using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Project.Data;
using Project.Models;

namespace Project.Repository
{
        public class LoginRepository : ILoginRepository{
            private readonly BancoContext _bancoContext;
            public LoginRepository(BancoContext bancoContext)
            {
                _bancoContext = bancoContext;
            }
           /* public CadastroModel Selecionar(LoginModel login){  
                return _bancoContext.Cadastro.FirstOrDefault(x => x.Email == login.Email);
            }*/

            public CadastroModel Selecionar(LoginModel login)
{
            CadastroModel usuario = _bancoContext.Cadastro.FirstOrDefault(x => x.Email == login.Email);
                if(usuario == null){
                    return null;
                }

                var perfilUsuario = _bancoContext.PerfilUsuario.FirstOrDefault(x => x.CadastroId == usuario.Id);

                if(perfilUsuario != null){
                    var perfil = _bancoContext.Perfil.FirstOrDefault(x => x.Id == perfilUsuario.PerfilId);
                    if(perfil != null){
                        usuario.Perfil = perfil.Nome;
                    }
                }

            return usuario;
        }

            public PerfilModel Buscar(PerfilModel perfil){
                return _bancoContext.Perfil.FirstOrDefault(x => x.Nome == perfil.Nome);
            }

        }


}