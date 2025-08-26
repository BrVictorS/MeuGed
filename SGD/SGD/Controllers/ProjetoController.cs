using LojaLivros.Services.Projeto;
using SGD.Data;
using SGD.Dtos.Projeto;
using SGD.Models;
using SGD.Services.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SGD.Controllers
{    
    public class ProjetoController:Controller
    {
        private readonly IProjetoInterface _projetoInterface;
        private readonly DataDbContext _dbContext;
        public ProjetoController(IProjetoInterface projetoInterface, DataDbContext dbContext)
        {
            _projetoInterface = projetoInterface;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index(int? Id)
        {
            var projetos = await _projetoInterface.BuscarProjetos();
            return View(projetos);
        }        
        public IActionResult Cadastrar()
        {
            ProjetoModel projeto = new ProjetoModel();
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar([FromBody] ProjetoModel model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Projeto.Add(model);
                _dbContext.SaveChanges();

                
                return Json(new { success = true, message = "Projeto criado com sucesso!" });
            }

            return Json(new { success = false, message = "Erro ao criar Projeto!" });
        }
        [HttpPost]
        public async Task<IActionResult> Editar([FromBody] ProjetoEditarDto projeto)
        {            
            var projetoExistente = _dbContext.Projeto.Any( f=> f.Id == projeto.Id);
            if (!projetoExistente)
            {
                return Json(new { success = false, message = "Projeto não encontrado." });
            }
            else
            {
                await _projetoInterface.Editar(projeto);
            }            
            
            return Json(new { success = true, message = "Projeto atualizado com sucesso!" });
        }            
        [HttpGet]
        public IActionResult BuscarUsuarios(string? term)
        {
            var usuarios = _projetoInterface.BuscarUsuarios(term);

            return Json(usuarios);
        }
        [HttpGet]
        public async Task<IActionResult> ListarUsuarios(int? id)
        {
            var usuarios = await _projetoInterface.ListarUsuarios(id);

            return Json(usuarios);
        }
        public IActionResult AdicionarUsuario(int idUsuario)
        {
            List<int> usus = ViewBag.UsuAdd;
            if (usus.Contains(idUsuario))
            {
                usus.Remove(idUsuario);
            }
            else
            {
                usus.Add(idUsuario);
            }

            return Json(new { success = true, message = "Projeto atualizado com sucesso!" });
        }
    }
}
