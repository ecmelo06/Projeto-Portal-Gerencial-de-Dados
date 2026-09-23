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

    public IActionResult AcessLogin()
    {
        string nome = HttpContext.Session.GetString("UsuarioNome");
        if (nome == null)
        {
            return RedirectToAction("ReturnToLogin");
        }
        return View("~/Views/UserAcess/Login.cshtml");
    }

    public IActionResult Cadastro()
    {
        return View("~/Views/Home/Cadastro.cshtml");
    }
    [HttpPost]
    public IActionResult Buscar(LoginModel login)
    {
        CadastroModel usuario = _loginRepository.Selecionar(login);
        
        if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
        {
            return RedirectToAction("ReturnToLogin");
        }
        if(HashHelper.VerificarHash(
            usuario.Senha,
            login.Senha)){
            HttpContext.Session.SetString(
                "UsuarioNome", 
                usuario.Nome
            );
            HttpContext.Session.SetString(
                "UsuarioEmail", 
                usuario.Email
            );
            HttpContext.Session.SetString(
                "UsuarioPerfil",
                usuario.Perfil
            );
            
            return View("~/Views/UserAcess/Login.cshtml",usuario);
            
        }   
        return RedirectToAction("ReturnToLogin");   
    }
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("ReturnToLogin");
    }
}