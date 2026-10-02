using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;
using Project.Helpers;

public class ControllerLogin : Controller
{
    private readonly ILoginRepository _loginRepository;
    public ControllerLogin(ILoginRepository loginRepository){
        _loginRepository = loginRepository;   
    }
    public IActionResult ReturnToLogin()
    {
        return View("~/Views/Home/Login.cshtml");
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
    
    public IActionResult Cadastro()
    {
        return View("~/Views/Home/Cadastro.cshtml");
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
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("ReturnToLogin");
    }
}