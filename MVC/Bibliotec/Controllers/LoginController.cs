using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bibliotec.Contexts;
using Bibliotec.Models;
using Bibliotec.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
        public string? Message { get; set; }

        Context c = new Context();

        public IActionResult Index()
        {
            return View();
        }

        private string GerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, usuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.Role, usuario.Admin.ToString())

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("bibliotec-chave-autenticacao-dev"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Bibliotec",
                audience: "Bibliotec",
                claims: claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: creds
            );

            string stringToken = new JwtSecurityTokenHandler().WriteToken(token);

            return stringToken;

        }

        [Route("Logar")]
        public IActionResult Logar(IFormCollection form)
        {
            string emailInformado = form["Email"].ToString();
            string senhaInformado = form["Senha"].ToString();


            Usuario usuarioBuscado = c.Usuario.FirstOrDefault(u => u.Email == emailInformado && u.Senha == senhaInformado)!;

            //Verificando se o usuário é tipo aluno ou tipo admin

            // if (usuarioBuscado.Admin == true)
            // {


            // }


            if (usuarioBuscado != null)
            {
                GerarToken(usuarioBuscado);
                string token = GerarToken(usuarioBuscado);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddYears(1)
                };

                Response.Cookies.Append("tokenBibliotec", token, cookieOptions);

                ValidarToken.Validar(HttpContext);

                // Console.WriteLine(ValidarToken.Validar(HttpContext));


                HttpContext.Session.SetString("UserName", usuarioBuscado.Nome!);
                HttpContext.Session.SetString("UserID", usuarioBuscado.UsuarioID.ToString());
                return LocalRedirect("~/Livro");
            }

            // Valida se for admin ele meio que ignora
            

            Message = "Dados inválidos!";
            Console.WriteLine($"Dados inválidos!");


            return LocalRedirect("~/Login");
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");

            return LocalRedirect("~/");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Erro")]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}