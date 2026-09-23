
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
        _cadastroRepository.Adicionar(cadastro);
        return RedirectToAction("AcessUser");
    }
}