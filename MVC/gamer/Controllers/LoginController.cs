using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using gamer.Contexts;
using gamer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace gamer.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        //mensagem padrão
        [TempData]
        public string Mensagem { get; set; }
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        Context c = new Context();

        [Route("Login")]
        public IActionResult Index()
        {
            //cria uma ViewBag obtendo os dados do usuário logado
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View();
        }

        //rota de logar, valida e retorna os dados do usuário logado na aplicação
        [Route("Logar")]
        public IActionResult Logar(IFormCollection form)
        {
            //recebe os dados do formulário do login e armazena nas variáveis
            string email = form["Email"].ToString();
            string senha = form["Senha"].ToString();

            //acessa o contexto, a tabela e faz uma busca passando como parâmetro o email e senha recebidos e armazena em um objeto
            Jogador jogadorBuscado = c.Jogador.FirstOrDefault(x => x.Email == email && x.Senha == senha);

            //se o objeto buscado existir ele estará logado
            if (jogadorBuscado != null)
            {
                //seta o nome do jogador logado no UserName
                HttpContext.Session.SetString("UserName", jogadorBuscado.Nome);
                return LocalRedirect("~/");
            }
            //atribuir um valor para a propriedade de mensagem
            Mensagem = "Dados inválidos!";
            return LocalRedirect("~/Login/Login");
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            return LocalRedirect("~/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}