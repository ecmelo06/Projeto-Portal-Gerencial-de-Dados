using Project.Models;

namespace Project.Repository
{
    public interface ILoginRepository
    {
        CadastroModel Selecionar(LoginModel selecionar);
    }
}
