
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;

public class ControllerCadastro : Controller{
    
    private readonly ICadastroRepository _cadastroRepository;

    public ControllerCadastro(ICadastroRepository cadastroRepository)
    {
        _cadastroRepository = cadastroRepository;   
        
    }
    public IActionResult Criar()
    {
        return View();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AcessUser()
    {
        return View("~/Views/Home/Login.cshtml");
    }
 
    [HttpPost]
    public IActionResult Criar(CadastroModel cadastro){
        if (string.IsNullOrWhiteSpace(cadastro.Nome))
        {
            TempData["Erro"] = "Nome não informado.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }
        if (string.IsNullOrWhiteSpace(cadastro.Senha))
        {
            TempData["Erro"] = "Senha não informada.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);  
        }
        if (string.IsNullOrWhiteSpace(cadastro.Email))
        {
            TempData["Erro"] = "E-mail não informado.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }
        if (string.IsNullOrWhiteSpace(cadastro.CPF))
        {
            TempData["Erro"] = "CPF não informado";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }
        if (string.IsNullOrWhiteSpace(cadastro.Orgao))
        {
            TempData["Erro"] = "Órgão não informado.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }
        _cadastroRepository.Adicionar(cadastro);
            TempData["Sucesso"] ="Usuário cadastrado com sucesso.";
        return RedirectToAction("AcessUser");
        
    }
}