using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Project.Data;
using Project.Models;
using Project.Helpers;

namespace Project.Repository
{

    
    public class CadastroRepository : ICadastroRepository
    {
        private readonly BancoContext _bancoContext;
        

    
        public CadastroRepository(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }
        public CadastroModel Adicionar(CadastroModel cadastro){

            cadastro.Senha = HashHelper.GerarHash(cadastro.Senha);
            _bancoContext.Cadastro.Add(cadastro);
            _bancoContext.SaveChanges();
        
            PerfilUsuario newPerfil = new PerfilUsuario{
                CadastroId = cadastro.Id, PerfilId = 3
            };

            _bancoContext.PerfilUsuario.Add(newPerfil);
            _bancoContext.SaveChanges();
           
           return cadastro;

        }
    }
}