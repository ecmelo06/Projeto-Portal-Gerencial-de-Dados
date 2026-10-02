
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Repository;
using System.Text.RegularExpressions;


public class ControllerCadastro : Controller{
    
    private readonly ICadastroRepository _cadastroRepository;
    string padraoSenha = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{10,}$";

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
        bool emailExiste = _cadastroRepository.ExisteEmail(cadastro.Email);

        if (emailExiste)
        {
            TempData["Erro"] = "O e-mail informado já está cadastrado.";
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
        
        string CPFLimpo = cadastro?.CPF?.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
        if(cadastro?.CPF?.Length != 11 || string.IsNullOrWhiteSpace(CPFLimpo))
        {
            TempData["Erro"] = "CPF inválido.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }
        string[] cpfsNumInvalidos = {"00000000000", "11111111111", "22222222222", "33333333333", "44444444444",
                                    "55555555555", "66666666666", "77777777777", "88888888888", "99999999999"};      
        cadastro.CPF = CPFLimpo;
        
        if (cpfsNumInvalidos.Contains(cadastro.CPF))
        {
            TempData["Erro"] = "CPF inválido.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }

        int soma1 = 0;
        int soma2 = 0;

        for (int i = 0; i < 9; i++){
            int digito = cadastro.CPF[i] - '0';
            soma1 += digito * (10 - i);
            soma2 += digito * (11 - i);
        }

        int resto1 = soma1 % 11;
        int digitoCalculado1 = resto1 < 2 ? 0 : 11 - resto1;

        soma2 += digitoCalculado1 * 2;

        int resto2 = soma2 % 11;
        int digitoCalculado2 = resto2 < 2 ? 0 : 11 - resto2;

        int digitoInformado1 = cadastro.CPF[9] - '0';
        int digitoInformado2 = cadastro.CPF[10] - '0';

        if (digitoCalculado1 != digitoInformado1 || digitoCalculado2 != digitoInformado2)
        {
            TempData["Erro"] = "O CPF informado é inválido.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }


        if (string.IsNullOrWhiteSpace(cadastro.Orgao))
        {
            TempData["Erro"] = "Órgão não informado.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }
        
        if(cadastro?.Senha?.Length < 10)
        {
            TempData["Erro"] = "Tamanho de senha inválida.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }  

        if (!Regex.IsMatch(cadastro.Senha, padraoSenha)){
            TempData["Erro"] = "A senha deve conter letras maiúsculas, minúsculas, números e caracteres especiais.";
            return View("~/Views/Home/Cadastro.cshtml", cadastro);
        }
        _cadastroRepository.Adicionar(cadastro);
            TempData["Sucesso"] ="Usuário cadastrado com sucesso.";
        return RedirectToAction("AcessUser");
        
    }
}