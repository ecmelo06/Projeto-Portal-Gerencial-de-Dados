using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Repository
{
    public interface ICadastroRepository
    {
        CadastroModel Adicionar(CadastroModel cadastro);
    }
}