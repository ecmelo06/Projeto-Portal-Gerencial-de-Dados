using Project.Models;

namespace Project.Repository
{
    public interface ILoginRepository
    {
        CadastroModel Selecionar(LoginModel selecionar);
        List<CadastroModel> BuscarTodos();

        PerfilModel Buscar(PerfilModel perfil);

        // --- NOVOS MÉTODOS ADICIONADOS ---
        CadastroModel BuscarPorId(int id);
        bool Atualizar(CadastroModel usuario);
        bool PromoverParaAdmin(int usuarioId);
        bool Excluir(int id);

        public bool VerificaEmail(string email, int idAtual);
        bool AlterarPerfilUsuario(int usuarioId, string nomeNovoPerfil);

        void SalvarCodigoRecuperacao(string email, string codigo);
        bool ValidarCodigo(string email, string codigo);
        void AtualizarSenha(string email, string novaSenhaHash);


    }
}
