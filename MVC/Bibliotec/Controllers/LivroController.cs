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
        [Route("Favoritar/{id}")]
        public IActionResult Favoritar(int id)
        {

            // Obter o nome do usuário logado na sessão
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                // Redirecionar para login, se o usuário não estiver autenticado
                return RedirectToAction("Login", "Usuario");
            }

            // Buscar o usuário pelo nome de usuário
            Usuario usuario = c.Usuario.FirstOrDefault(u => u.Nome == userName)!;

            if (usuario == null)
            {
                // Caso o usuário não seja encontrado, retornar um erro
                return NotFound("Usuário não encontrado");
            }

            // Buscar o livro específico pelo ID
            Livro livro = c.Livro.FirstOrDefault(j => j.LivroID == id)!;

            if (livro == null)
            {
                // Caso o livro não seja encontrado, retornar um erro
                return NotFound("Livro não encontrado");
            }

            // Verificar se o livro já está favoritado pelo usuário
            bool jaFavoritado = c.LivroFavorito.Any(f => f.LivroID == id && f.UsuarioID == usuario.UsuarioID);

            Console.WriteLine($"{jaFavoritado}");


            if (!jaFavoritado)
            {
                // Adicionar o livro à tabela LivroFavoritos com o ID do usuário
                LivroFavorito favorito = new LivroFavorito
                {
                    LivroID = id,
                    UsuarioID = usuario.UsuarioID
                };

                c.LivroFavorito.Add(favorito);
                c.SaveChanges();
            }

            // Redirecionar para a página de livros ou outra página desejada
            return LocalRedirect("~/Livro");
        }


        // Criar método para favoritar o livro
        [Route("Favoritos")]
        public IActionResult Favoritos()
        {

            // Obter o nome do usuário logado na sessão
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                // Redirecionar para login se o usuário não estiver autenticado
                return RedirectToAction("Login", "Usuario");
            }

            // Buscar o usuário pelo nome de usuário
            Usuario usuario = c.Usuario.FirstOrDefault(u => u.Nome == userName)!;

            if (usuario == null)
            {
                // Caso o usuário não seja encontrado, retornar um erro
                return NotFound("Usuário não encontrado");
            }

            // Buscar os livros favoritados pelo usuário
            var livrosFavoritos = c.LivroFavorito
                .Where(f => f.UsuarioID == usuario.UsuarioID)
                .Select(f => f.Livro) // Obter apenas os dados dos livros
                .ToList();

            // Passar os livros favoritados para a view
            ViewBag.LivrosFavoritos = livrosFavoritos;

            return View();
        }


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
            // List para armazenar múltiplas categorias
            List<LivroCategoria> livroCategorias = new List<LivroCategoria>();

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

            string[] categoriaIds = form["CategoriaID"].ToString().Split(',');

            foreach (string categoriaId in categoriaIds)
            {
                LivroCategoria livroCategoria = new LivroCategoria();
                livroCategoria.CategoriaID = int.Parse(categoriaId);
                livroCategoria.LivroID = novoLivro.LivroID;
                livroCategorias.Add(livroCategoria);
            }
            c.LivroCategoria.AddRange(livroCategorias);
            c.SaveChanges();

            return LocalRedirect("~/Livro");
        }

        //Criar método para editar o livro:
        [Route("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            // ListaCategoria();

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // Buscar o livro específico pelo ID
            Livro livro = c.Livro.FirstOrDefault(j => j.LivroID == id)!;

            // Buscar as categorias relacionadas ao livro
            var categoriasDoLivro = c.LivroCategoria
              .Where(lc => lc.LivroID == id)
              .Select(lc => lc.Categoria)
              .ToList();

            // Passar as informações para a view
            ViewBag.Livro = livro;
            ViewBag.categoriasDoLivro = categoriasDoLivro;
            ViewBag.Categoria = c.Categoria.ToList();

            return View("Edit");
        }


        [Route("Atualizar/{id}")]
        public IActionResult Atualizar(int id, IFormCollection form, IFormFile? imagem)
        {
            // Buscar o livro específico pelo ID
            Livro livro = c.Livro.FirstOrDefault(j => j.LivroID == id);

            if (livro == null)
            {
                return NotFound(); // Retorna erro 404 se o livro não for encontrado
            }

            // Atualizar os dados do livro com as informações do formulário
            livro.Nome = form["Nome"];
            livro.Descricao = form["Descricao"];
            livro.Editora = form["Editora"];
            livro.Escritor = form["Escritor"];
            livro.Idioma = form["Idioma"];

            // Atualizar a imagem, se uma nova for fornecida
            if (imagem != null && imagem.Length > 0)
            {
                // Caminho onde a imagem será salva
                var caminhoImagem = Path.Combine("wwwroot/img/Livros", imagem.FileName);

                // Excluir a imagem antiga, se existir
                if (!string.IsNullOrEmpty(livro.Imagem))
                {
                    var caminhoAntigo = Path.Combine("wwwroot/img/Livros", livro.Imagem);
                    if (System.IO.File.Exists(caminhoAntigo))
                    {
                        System.IO.File.Delete(caminhoAntigo);
                    }
                }

                // Salvar a nova imagem no caminho especificado
                using (var stream = new FileStream(caminhoImagem, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }

                // Atualizar o caminho da imagem no banco de dados
                livro.Imagem = imagem.FileName;
            }

            // Atualizar categorias
            var categoriasSelecionadas = form["CategoriaID"].ToList();
            var categoriasDoLivro = c.LivroCategoria.Where(lc => lc.LivroID == id).ToList();

            // Remover categorias que não estão mais selecionadas
            foreach (var categoria in categoriasDoLivro)
            {
                if (!categoriasSelecionadas.Contains(categoria.CategoriaID.ToString()))
                {
                    c.LivroCategoria.Remove(categoria);
                }
            }

            // Adicionar novas categorias selecionadas
            foreach (var categoriaId in categoriasSelecionadas)
            {
                if (!categoriasDoLivro.Any(c => c.CategoriaID.ToString() == categoriaId))
                {
                    c.LivroCategoria.Add(new LivroCategoria
                    {
                        LivroID = id,
                        CategoriaID = int.Parse(categoriaId)
                    });
                }
            }

            // Salvar alterações no banco de dados
            c.SaveChanges();

            // Redirecionar para a página de detalhes ou outra página desejada
            return RedirectToAction("Detalhes", new { id });
        }


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