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
    public ControllerLogin(ILoginRepository loginRepository, ICadastroRepository cadastroRepository){
        _loginRepository = loginRepository;   
        _cadastroRepository = cadastroRepository;
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

    public IActionResult EsqueciSenha() {
        return View("~/Views/Home/EsqueciSenha.cshtml");
    }
    public IActionResult InserirCodigo() {
        return View("~/Views/Home/InserirCodigo.cshtml"); 
    }


    [NoCache]
    public IActionResult AcessAdmin(){
    // 1. Recupera o e-mail do usuário logado através da sessão
    string email = HttpContext.Session.GetString("UsuarioEmail");
    if (string.IsNullOrWhiteSpace(email))
    {
        return RedirectToAction("ReturnToLogin");
    }

    // 2. Busca os dados e as tags de perfis deste usuário específico
    LoginModel loginTemporario = new LoginModel { Email = email };
    CadastroModel usuarioLogado = _loginRepository.Selecionar(loginTemporario);

    if (usuarioLogado != null)
    {
        // Alimenta as ViewBags que a View precisa ler no topo da tela
        ViewBag.NomeUsuarioLogado = usuarioLogado.Nome;
        ViewBag.PerfisUsuarioLogado = usuarioLogado.Perfils != null 
            ? string.Join(", ", usuarioLogado.Perfils.Select(p => p.Nome)) 
            : "Nenhum";
    }
    else
    {
        ViewBag.NomeUsuarioLogado = "Administrador";
        ViewBag.PerfisUsuarioLogado = "Admin";
    }

    // 3. Busca todos os registros para preencher as linhas da tabela Bootstrap
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

    [NoCache]
    public IActionResult AcessLogin(){
        // 1. Recupera o e-mail de quem está logado na sessão
        string email = HttpContext.Session.GetString("UsuarioEmail");

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("ReturnToLogin");
        }

        // 2. Busca os dados DESTE usuário específico no banco
        LoginModel loginTemporario = new LoginModel { Email = email };
        CadastroModel usuario = _loginRepository.Selecionar(loginTemporario);

        if (usuario == null)
        {
            TempData["Erro"] = "Usuário não encontrado no sistema.";
            return RedirectToAction("ReturnToLogin");
        }

        // 3. Preenche as ViewBags para o topo da tela do usuário (caso a tela comum use o mesmo layout de card)
        ViewBag.NomeUsuarioLogado = usuario.Nome;
        ViewBag.PerfisUsuarioLogado = usuario.Perfils != null 
            ? string.Join(", ", usuario.Perfils.Select(p => p.Nome)) 
            : "Nenhum";

        // 4. RETORNO CORRETO: Passa apenas o objeto 'usuario' (e não a lista 'valores') 
        // e aponta para a View de acesso do Usuário Comum
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
                if(usuario.Perfils.Any(p => p.Nome == "Admin" || p.Nome == "SuperAdmin")){
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
            TempData["Erro"] = "Não foi possível atualizar: O e-mail informado já está cadastrado.";
            return View("~/Views/Admin/Editar.cshtml", usuario);
        }

        bool atualizou = _loginRepository.Atualizar(usuario);

            if (atualizou){
                TempData["Sucesso"] = "Usuário atualizado com sucesso no banco de dados!";
                return RedirectToAction("AcessAdmin");
            }
    
            TempData["Erro"] = "Falha: Não foi possível salvar as alterações no banco.";
            return View("~/Views/Admin/Editar.cshtml", usuario);
        }


        public IActionResult TornarAdmin(int id)
        {
            _loginRepository.PromoverParaAdmin(id);
            TempData["Sucesso"] = "Usuário promovido a Admin.";
            return RedirectToAction("AcessAdmin");
        }

        public IActionResult ExcluirUsuario(int id){
            string emailLogado = HttpContext.Session.GetString("UsuarioEmail");
            LoginModel loginTemporario = new LoginModel { Email = emailLogado };
            CadastroModel operadorLogado = _loginRepository.Selecionar(loginTemporario);

            if (operadorLogado == null) return RedirectToAction("ReturnToLogin");

            CadastroModel alvo = _loginRepository.BuscarPorId(id);
            if (alvo == null)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                return RedirectToAction("AcessAdmin");
            }

            if (alvo.Email == emailLogado)
            {
                TempData["Erro"] = "Segurança: Você não pode excluir a sua própria conta administrativa.";
                return RedirectToAction("AcessAdmin");
            }

            // CORREÇÃO DA REGRA: Validação por nome do perfil em vez de ID fixo
            bool ehSuperAdminLogado = operadorLogado.Perfils != null && 
                                    operadorLogado.Perfils.Any(p => p.Nome == "SuperAdmin");
                                    
            bool alvoEhAdmin = alvo.Perfils != null && 
                            alvo.Perfils.Any(p => p.Nome == "Admin" || p.Nome == "SuperAdmin");

            if (alvoEhAdmin && !ehSuperAdminLogado)
            {
                TempData["Erro"] = "Segurança: Apenas um SuperAdmin pode excluir outro administrador do sistema.";
                return RedirectToAction("AcessAdmin");
            }

            _loginRepository.Excluir(id);
            TempData["Sucesso"] = "Usuário excluído com sucesso.";
            return RedirectToAction("AcessAdmin");
        }


        public IActionResult MudarPerfil(int id, string novoPerfil){
            string emailLogado = HttpContext.Session.GetString("UsuarioEmail");
            LoginModel loginTemporario = new LoginModel { Email = emailLogado };
            CadastroModel operadorLogado = _loginRepository.Selecionar(loginTemporario);

            if (operadorLogado == null) return RedirectToAction("ReturnToLogin");

            CadastroModel alvo = _loginRepository.BuscarPorId(id);
            if (alvo == null) return RedirectToAction("AcessAdmin");

            // Impede qualquer alteração no próprio usuário logado (evita se auto-rebaixar por engano)
            if (alvo.Email == emailLogado)
            {
                TempData["Erro"] = "Não é permitido alterar o seu próprio nível de acesso enquanto estiver logado.";
                return RedirectToAction("AcessAdmin");
            }

            // CORREÇÃO DA REGRA: Procura se a palavra "SuperAdmin" existe nas permissões de quem está logado
            bool ehSuperAdminLogado = operadorLogado.Perfils != null && 
                                    operadorLogado.Perfils.Any(p => p.Nome == "SuperAdmin");
                                    
            bool alvoEhAdmin = alvo.Perfils != null && 
                            alvo.Perfils.Any(p => p.Nome == "Admin" || p.Nome == "SuperAdmin");

            // Se o alvo for um Administrador e quem está operando NÃO for um SuperAdmin, bloqueia!
            if (alvoEhAdmin && !ehSuperAdminLogado)
            {
                TempData["Erro"] = "Segurança: Apenas usuários com nível de SuperAdmin podem rebaixar ou alterar o cargo de outro Administrador.";
                return RedirectToAction("AcessAdmin");
            }

            // Se passou na validação, executa a mudança no banco
            _loginRepository.AlterarPerfilUsuario(id, novoPerfil);
            TempData["Sucesso"] = $"Perfil do usuário atualizado para {novoPerfil} com sucesso!";
            return RedirectToAction("AcessAdmin");
        }        

        [HttpPost]
        public async Task<IActionResult> EnviarEmailRecuperacao(string email){
            bool existe = _cadastroRepository.ExisteEmail(email);
            if (!existe)
            {
                TempData["Erro"] = "E-mail não cadastrado no sistema.";
                return RedirectToAction("EsqueciSenha");
            }

            // Gera um código numérico aleatório de 6 dígitos
            string codigoGerado = new Random().Next(100000, 999999).ToString();

            // Salva no banco e envia o e-mail
            _loginRepository.SalvarCodigoRecuperacao(email, codigoGerado);
            await EmailService.EnviarCodigoRecuperacao(email, codigoGerado);

            // Salva o e-mail temporariamente na sessão para validar no próximo passo
            HttpContext.Session.SetString("EmailEmRecuperacao", email);

            TempData["Sucesso"] = "Código enviado com sucesso para o seu e-mail.";
            return RedirectToAction("InserirCodigo");
        }

        // ETAPA 2: Tela para digitar o código recebido
        [HttpPost]
        public IActionResult ValidarCodigoRecuperacao(string codigo)
        {
            string email = HttpContext.Session.GetString("EmailEmRecuperacao");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("ReturnToLogin");

            bool codigoValido = _loginRepository.ValidarCodigo(email, codigo);

            if (codigoValido)
            {
                // Código correto, permite avançar para a última tela
                HttpContext.Session.SetString("CodigoValidado", "true");
                return RedirectToAction("NovaSenha");
            }

            TempData["Erro"] = "Código inválido ou expirado.";
            return RedirectToAction("InserirCodigo");
        }

        // ETAPA 3: Tela para digitar a nova senha
        public IActionResult NovaSenha()
        {
            // Proteção: Só entra se passou pela validação do código
            if (HttpContext.Session.GetString("CodigoValidado") != "true") return RedirectToAction("ReturnToLogin");
                return View("~/Views/Home/NovaSenha.cshtml");               
        }

        [HttpPost]
        public IActionResult SalvarNovaSenha(string novaSenha, string confirmarSenha)
        {
            string email = HttpContext.Session.GetString("EmailEmRecuperacao");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("ReturnToLogin");

            if (novaSenha != confirmarSenha)
            {
                TempData["Erro"] = "As senhas não coincidem.";
                return View("NovaSenha");
            }

            // Insira aqui sua validação Regex de força de senha se desejar (a mesma do cadastro)

            // Criptografa e atualiza no banco
            string novaSenhaHash = HashHelper.GerarHash(novaSenha);
            _loginRepository.AtualizarSenha(email, novaSenhaHash);

            // Limpa as variáveis da sessão temporária
            HttpContext.Session.Remove("EmailEmRecuperacao");
            HttpContext.Session.Remove("CodigoValidado");

            TempData["Sucesso"] = "Senha alterada com sucesso! Faça login com a nova senha.";
            return RedirectToAction("ReturnToLogin");
        }



    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return RedirectToAction("ReturnToLogin");
    }
}