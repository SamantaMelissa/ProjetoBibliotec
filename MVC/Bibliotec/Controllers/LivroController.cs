using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec.Contexts;
using Bibliotec.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bibliotec.Controllers
{
    [Route("[controller]")]
    public class LivroController : Controller
    {
        // public LivroController()
        // {
        // }
        private readonly ILogger<LivroController> _logger;

        public LivroController(ILogger<LivroController> logger)
        {
            _logger = logger;
        }


        Context c = new Context();

        //Criar método para listar os livros:
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

            //Procurar o id na CategoriaLivro:

            var categoriasDoLivro = c.LivroCategoria
                .Where(lc => lc.LivroID == id)
                .Select(lc => c.Categoria.First(c => c.CategoriaID == lc.CategoriaID))
                .ToList();

            ViewBag.Livro = livroUnico;
            ViewBag.Categoria = categoriasDoLivro;

            return View("Detalhes");
        }

        // Criar método para favoritar o livro
        // public string Favoritar(string parameter)
        // {

        //     return System.NotImplementedException;
        // }


        // Método que lista nossas categorias:
        [Route("Categorias")]
        public IActionResult ListaCategoria()
        {
            ViewBag.Categoria = c.Categoria.ToList();

            return View();
        }

        //Criar método para ir para nossa tela de cadastro:
        [Route("Cadastro")]
        // [Authorize]
        public IActionResult Cadastro()
        {
            ListaCategoria();

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
            // novoLivro.Imagem = form["Imagem"].ToString();

            Console.WriteLine(form.Files.Count);


            // upload inicio
            // array = 0 sem arquivo array > 0 com arquivo
            if (form.Files.Count > 0)
            {
                // Armazena o array dentro da variavel:
                var file = form.Files[0];

                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Livros");

                // Verifica se a pasta existe, se n existe, ele cria:

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/", folder, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                novoLivro.Imagem = file.FileName;

            }
            else
            {

                novoLivro.Imagem = "padrao.png";

            }

            // upload final

            c.Livro.Add(novoLivro);

            c.SaveChanges();

            livroCategoria.CategoriaID = int.Parse(form["CategoriaID"].ToString());
            livroCategoria.LivroID = novoLivro.LivroID;
            // Adicionando uma nova categoria:
            c.LivroCategoria.Add(livroCategoria);
            c.SaveChanges();

            return LocalRedirect("~/Livro");
        }


        //Criar método para editar o livro:
        [Route("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            // ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // ListaCategoria();

            // Livro novoLivro = new Livro();
            // LivroCategoria livroCategoria = new LivroCategoria();

            // Livro livro = c.Livro.First(j => j.LivroID == id);
            // var categoriasDoLivro = c.LivroCategoria.Where(lc => lc.LivroID == id).ToList();


            // ViewBag.Categoria = categoriasDoLivro;
            // ViewBag.Livro = c.Livro.ToList();

             ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // Buscar o livro específico pelo ID
            Livro livro = c.Livro.FirstOrDefault(j => j.LivroID == id)!;

            // Buscar as categorias relacionadas ao livro
            var categoriasDoLivro = c.LivroCategoria
                .Where(lc => lc.LivroID == id)
                .Select(lc => lc.Categoria)
                .ToList();

            // Buscar todas as categorias
            // var todasAsCategorias = c.Categoria.ToList();

            var todasAsCategorias = c.Categoria.ToList();
            // Passar as informações para a view
            ViewBag.Livro = livro;
            ViewBag.Categoria = categoriasDoLivro;
            ViewBag.LivroCategoria = todasAsCategorias;
            return View("Editar");
        }


        // [Route("Atualizar")]
        // public IActionResult Atualizar(IFormCollection form)
        // {
        //     Livro novoLivro = new Livro();
        //     LivroCategoria livroCategoria = new LivroCategoria();

        //     novoLivro.Nome = form["Nome"].ToString();
        //     novoLivro.Descricao = form["Descricao"].ToString();
        //     novoLivro.Editora = form["Editora"].ToString();
        //     novoLivro.Escritor = form["Escritor"].ToString();
        //     novoLivro.Idioma = form["Idioma"].ToString();

        //     Livro livroBuscado = c.Livro.First(j => j.LivroID == novoLivro.LivroID);

        //     livroBuscado.Nome = novoLivro.Nome;
        //     livroBuscado.Descricao = novoLivro.Descricao;
        //     livroBuscado.Editora = novoLivro.Editora;
        //     livroBuscado.Escritor = novoLivro.Escritor;
        //     livroBuscado.Idioma = novoLivro.Idioma;

        //     c.Livro.Update(livroBuscado);
        //     c.SaveChanges();

        //     return LocalRedirect("~/Jogador/Listar");
        // }


        //Criar método para EXCLUIR o livro:
        [Route("Excluir/{id}")]
        public IActionResult Excluir(int id)
        {
            Livro livro = c.Livro.First(j => j.LivroID == id);

            // Obter as entradas de LivroCategoria relacionadas ao livro
            var categoriasDoLivro = c.LivroCategoria.Where(lc => lc.LivroID == id).ToList();

            // Remover as entradas de LivroCategoria
            foreach (var livroCategoria in categoriasDoLivro)
            {
                c.LivroCategoria.Remove(livroCategoria);
            }

            // Remover o livro
            c.Livro.Remove(livro);

            // Salvar as alterações no banco de dados
            c.SaveChanges();

            return LocalRedirect("~/Livro");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Erro")]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}