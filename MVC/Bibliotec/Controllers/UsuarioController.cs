using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec.Contexts;
using Bibliotec.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bibliotec.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        Context c = new Context();

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            int id = int.Parse(HttpContext.Session.GetString("UserID")!);

            Usuario usuario = c.Usuario.FirstOrDefault(x => x.UsuarioID == id)!;

            if (usuario == null)
            {
                return NotFound();
            }
            Curso curso = c.Curso.FirstOrDefault(x => x.CursoID == usuario.CursoID)!;

            if (curso == null)
            {
                return NotFound("Curso não encontrado para o usuário.");
            }

            ViewBag.Usuario = usuario;

            ViewBag.Nome = usuario.Nome;
            ViewBag.Email = usuario.Email;
            ViewBag.Curso = curso.Nome;
            ViewBag.Periodo = curso.Periodo;
            ViewBag.Contato = usuario.Contato;
            ViewBag.DtNascimento = usuario.DtNascimento.ToString("dd/MM/yyyy");

            return View();
        }

        [Route("Cadastro")]
        public IActionResult Cadastro()
        {
            ViewBag.Curso = c.Curso.ToList();

            return View();
        }

        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {
            Usuario novoUsuario = new Usuario();

            bool validador;

            if (form["Admin"] == "on"){
                Console.WriteLine($"Checkado");
                validador = true;
            }else{
                validador = false;
            }

            novoUsuario.Nome = form["Nome"].ToString();
            novoUsuario.Contato = form["Contato"].ToString();
            novoUsuario.Email = form["Email"].ToString();
            novoUsuario.Senha = "123";
            novoUsuario.Admin = validador;
            novoUsuario.Status = true;
            novoUsuario.DtNascimento = DateOnly.Parse(form["DtNascimento"]);

            if (form["CursoID"] == ""){
                novoUsuario.CursoID = null;
            }
            else{
                novoUsuario.CursoID = int.Parse(form["CursoID"]);
            }

            c.Usuario.Add(novoUsuario);

            c.SaveChanges();

            return LocalRedirect("~/Usuario/Cadastro");
        }

        [Route("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // Buscar o livro específico pelo ID
            Usuario usuario = c.Usuario.FirstOrDefault(x => x.UsuarioID == id)!;
            Curso cursoDoUsuario = c.Curso.FirstOrDefault(x => x.CursoID == usuario.CursoID)!;

            // Passar as informações para a view
            ViewBag.Usuario = usuario;
            ViewBag.cursoDoUsuario = cursoDoUsuario;
            // ViewBag.DtNascimento = usuario.DtNascimento.ToString("yyyy-MM-dd");
            ViewBag.Curso = c.Curso.ToList();

            return View("Edit");
        }

        [Route("Atualizar/{id}")]
        public IActionResult Atualizar(int id, IFormCollection form)
        {
            Usuario usuario = c.Usuario.FirstOrDefault(x => x.UsuarioID == id)!;

            // Atualizar os dados do livro com as informações do formulário
            usuario.Nome = form["Nome"];
            usuario.Email = form["Email"];
            usuario.Contato = form["Contato"];
            usuario.DtNascimento = DateOnly.Parse(form["DtNascimento"]);
            usuario.CursoID = int.Parse(form["CursoID"]);

            // Salvar alterações no banco de dados
            c.SaveChanges();

            return LocalRedirect("~/Usuario");
        }

        [Route("Excluir/{id}")]
        public IActionResult Excluir(int id)
        {
            Usuario usuario = c.Usuario.First(j => j.UsuarioID == id);

            // Obter as entradas de LivroCategoria relacionadas ao livro
            var favoritos = c.LivroFavorito.Where(lc => lc.UsuarioID == id).ToList();
            var reservados = c.LivroReserva.Where(lc => lc.UsuarioID == id).ToList();

            // Remover as entradas de LivroCategoria
            if (favoritos != null)
            {
                foreach (var usuarioFavoritos in favoritos)
                {
                    c.LivroFavorito.Remove(usuarioFavoritos);
                }

            }

            if (favoritos != null)
            {
                foreach (var usuarioReservados in reservados)
                {
                    c.LivroReserva.Remove(usuarioReservados);
                }
            }

            // Remover o livro
            c.Usuario.Remove(usuario);

            // Salvar as alterações no banco de dados
            c.SaveChanges();

            return LocalRedirect("~/Livro");
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}