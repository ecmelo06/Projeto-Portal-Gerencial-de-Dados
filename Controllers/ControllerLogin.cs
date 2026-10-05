using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;
using Project.Helpers;

public class ControllerLogin : Controller
{
    private readonly ICadastroRepository _cadastroRepository;
    private readonly ILoginRepository _loginRepository;
    public ControllerLogin(ILoginRepository loginRepository){
        _loginRepository = loginRepository;   
    }
    public IActionResult ReturnToLogin(){
        return View("~/Views/Home/Login.cshtml");
    }
     public IActionResult Cadastro(){
        return View("~/Views/Home/Cadastro.cshtml");
    }
    public IActionResult Editar(int id){
        CadastroModel usuario = _loginRepository.BuscarPorId(id);
        if (usuario == null) return RedirectToAction("AcessAdmin");
        return View("~/Views/Admin/Editar.cshtml", usuario);
    }

    public IActionResult AcessAdmin(){
        string email = HttpContext.Session.GetString("UsuarioEmail");
        if (string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction("ReturnToLogin");
        }
        List<CadastroModel> valores = _loginRepository.BuscarTodos();

        return View("~/Views/Admin/AcessAdmin.cshtml", valores);

    }

    public IActionResult AcessSuperAdmin()
    {
        string email = HttpContext.Session.GetString("UsuarioEmanil");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("ReturnToLogin");
        }
        return View("~/Views/Admin/AcessAdmin.cshtml");
    }

    public IActionResult AcessLogin(){
        string email = HttpContext.Session.GetString("UsuarioEmail");

        if (string.IsNullOrEmpty(email)){
            return RedirectToAction("ReturnToLogin");
        }

        LoginModel loginTemporario = new LoginModel { Email = email };
        CadastroModel usuario = _loginRepository.Selecionar(loginTemporario);


        if (usuario == null){
            TempData["Erro"] = "Usuário não encontrado.";
            return RedirectToAction("ReturnToLogin");
        }
        return View("~/Views/UserAcess/Login.cshtml", usuario);
    }

    public IActionResult RetornoValores()
    {
        string email = HttpContext.Session.GetString("UsuarioEmail");
        if (string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction("ReturnToLogin");
        }
        List<CadastroModel> valores = _loginRepository.BuscarTodos();

        return View("~/Views/Admin/AcessAdmin.cshtml", valores);
    }
    
    [HttpPost]
    public IActionResult Buscar(LoginModel login)
    {
        CadastroModel usuario = _loginRepository.Selecionar(login);
        
        if (string.IsNullOrWhiteSpace(login.Email))
        {
            TempData["Erro"] = "E-mail não digitado";
            return RedirectToAction("ReturnToLogin");
        }
        if (string.IsNullOrWhiteSpace(login.Senha))
        {
            TempData["Erro"] = "Senha não digitada";
            return RedirectToAction("ReturnToLogin");
        }
        if (usuario != null){
            if(HashHelper.VerificarHash(
                usuario.Senha,
                login.Senha))
                {
                HttpContext.Session.SetString(
                    "UsuarioEmail", 
                    usuario.Email
                ); 
                if(usuario.Perfils.Any(p => p.Nome == "Admin")){
                    return RedirectToAction("AcessAdmin");
                }       
                if (usuario.Perfils.Any(s => s.Nome == "SuperAdmin")){
                    return RedirectToAction("AcessAdmin");
                }       
                return RedirectToAction("AcessLogin");
            }
            
            else
            {
                TempData["Erro"] = "Senha inválida";
                return RedirectToAction("ReturnToLogin");
            }   
        }
        if(usuario == null)
        {
            TempData["Erro"] = "E-mail inválido";
            return RedirectToAction("ReturnToLogin");
        }
        TempData["Erro"] = "Usuário não encontrado";
        return RedirectToAction("ReturnToLogin");      
    }

        [HttpPost]
        public IActionResult Editar(CadastroModel usuario){
        bool emailJaCadastrado = _loginRepository.VerificaEmail(usuario.Email, usuario.Id);

        if (emailJaCadastrado)
        {
            // Define a mensagem de erro que será exibida no topo do formulário
            TempData["Erro"] = "Não foi possível atualizar: O e-mail informado já está cadastrado em outra conta.";
            return View("~/Views/Admin/Editar.cshtml", usuario);
        }

        bool atualizou = _loginRepository.Atualizar(usuario);

            if (atualizou){
                TempData["Sucesso"] = "Usuário atualizado com sucesso no banco de dados!";
                return RedirectToAction("AcessAdmin");
            }
    
            TempData["Erro"] = "Falha crítica: O repositório não conseguiu salvar as alterações no banco.";
            return View("~/Views/Admin/Editar.cshtml", usuario);
        }


        public IActionResult TornarAdmin(int id)
        {
            _loginRepository.PromoverParaAdmin(id);
            TempData["Sucesso"] = "Usuário promovido a Admin.";
            return RedirectToAction("AcessAdmin");
        }

        public IActionResult ExcluirUsuario(int id)
        {
            CadastroModel alvo = _loginRepository.BuscarPorId(id);
            if (alvo == null)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                return RedirectToAction("AcessAdmin");
            }

            // REGRA: Não pode excluir outro administrador
            if (alvo.Perfils.Any(p => p.Nome == "Admin" || p.Nome == "SuperAdmin"))
            {
                TempData["Erro"] = "Segurança: Não é permitido excluir outro administrador do sistema.";
                return RedirectToAction("AcessAdmin");
            }

            _loginRepository.Excluir(id);
            TempData["Sucesso"] = "Usuário excluído com sucesso.";
            return RedirectToAction("AcessAdmin");
        }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("ReturnToLogin");
    }
}