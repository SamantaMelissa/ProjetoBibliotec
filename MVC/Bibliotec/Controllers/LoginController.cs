using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec.Contexts;
using Bibliotec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bibliotec.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [TempData]
        public string? Message {get;set;}

        Context c = new Context();

        [Route("Login")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Logar")]
        public IActionResult Logar(IFormCollection form)
        {
            string emailInformado = form["Email"].ToString();
            string senhaInformado = form["Senha"].ToString();

            
            Usuario usuarioBuscado = c.Usuario.FirstOrDefault(j => j.Email == emailInformado && j.Senha == senhaInformado)!;

            
            if (usuarioBuscado != null)
            {
                HttpContext.Session.SetString("UserName", usuarioBuscado.Nome);
                return LocalRedirect("~/Livro/Lista");
            }

            Message = "Dados inválidos!";
            Console.WriteLine($"Dados inválidos!");
            

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