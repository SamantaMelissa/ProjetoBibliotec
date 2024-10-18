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
    public class LivroController : Controller
    {
        private readonly ILogger<LivroController> _logger;

        public LivroController(ILogger<LivroController> logger)
        {
            _logger = logger;
        }

        Context c = new Context();

        //Criar método para listar os livros:
        [Route("Lista")]
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Livro = c.Livro.ToList();
            return View();
        }

        //Criar método para mostrar os detalhes dos livros:
        [Route("Detalhes/{id}")]
        public IActionResult Detalhes(int id)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            Livro livroUnico = c.Livro.First(x => x.LivroID == id);

            ViewBag.Livro = livroUnico;

            return View("Detalhes");
        }


        //Criar método para filtrar o livro:

    
        [Route("Cadastro")]
        public IActionResult Cadastro()
        {
            ListaCategoria();

            return View();
        }


        [Route("Categorias")]
        public IActionResult ListaCategoria()
        {
            ViewBag.Categoria = c.Categoria.ToList();

            return View();
        }



        //Criar método para cadastrar o livro:
        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {
            Livro novoLivro = new Livro();
            LivroCategoria livroCategoria = new LivroCategoria();

            novoLivro.Nome = form["Nome"].ToString();
            novoLivro.Descricao = form["Descricao"].ToString();
            novoLivro.Editora = form["Editora"].ToString();
            novoLivro.Escritor = form["Escritor"].ToString();
            novoLivro.Idioma = form["Idioma"].ToString();
            novoLivro.Imagem = form["Imagem"].ToString();
            // categorias.Nome = form["Categoria"].ToString();

            c.Livro.Add(novoLivro);
            
            c.SaveChanges();


            // Console.WriteLine(novoLivro.LivroID);
            livroCategoria.CategoriaID = int.Parse(form["CategoriaID"].ToString()); 
            livroCategoria.LivroID = novoLivro.LivroID; 
            // Adicionando uma nova categoria:
            
        
            // Adicionando na tabela intermediária
     
            c.LivroCategoria.Add(livroCategoria);
            c.SaveChanges();
            
            return LocalRedirect("~/Livro/Lista");
        }


        //Criar método para editar o livro:
        [Route("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            // ViewBag.UserName = HttpContext.Session.GetString("UserName");
            
            // Jogador jogador = c.Jogador.First(j => j.IdJogador == id);

            // ViewBag.Jogador = jogador;

            // ViewBag.Equipe = c.Equipe.ToList();

            return View("Edit");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}