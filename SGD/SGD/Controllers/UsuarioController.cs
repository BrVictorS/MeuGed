using AutoMapper;
using SGD.Data;
using SGD.Dtos.Usuarios;
using SGD.Models;
using SGD.Services.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SGD.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioInterface _usuarioInterface;
        private readonly IMapper _mapper;
        private readonly DataDbContext _dbContext;

        public UsuarioController(IUsuarioInterface usuarioInterface, IMapper mapper, DataDbContext context)
        {
            _usuarioInterface = usuarioInterface;
            _mapper = mapper;
            _dbContext = context;
        }

        public async Task<IActionResult> Index(int? Id)
        {
            var usuarios = await _usuarioInterface.BuscarUsuarios(null);
            

            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            UsuarioRegisterDto usuarioRegisterDto = new UsuarioRegisterDto();            

            return View(usuarioRegisterDto);
        }


        [HttpGet]
        public IActionResult RedefinirSenha(int id)
        {
            return View();
        }


        public async Task<IActionResult> RedefinirSenha(int id, string senha)
        {
            if (id != null)
            {
                var usuario = await _usuarioInterface.BuscarUsuarioPorId(id);

                bool redefinido = await _usuarioInterface.RedefinirSenha(id, senha);

                if (redefinido)
                {
                    TempData["MensagemSucesso"] = "Senha redefinida com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Falha ao redefinir senha!";
                }

            }
            return RedirectToAction("Index", "Funcionario");

        }

        public async Task<IActionResult> Detalhes(int? Id)
        {
            if (Id != null)
            {
                var usuario = await _usuarioInterface.BuscarUsuarioPorId(Id);



                return View(usuario);
            }
            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> MudarSituacaoUsuario(UsuarioModel usuario)
        {
            if (usuario.Id != 0 && usuario.Id != null)
            {
                var usuarioBanco = await _usuarioInterface.MudarSituacaoUsuario(usuario.Id);

                if (usuarioBanco.Situação == true)
                {
                    TempData["MensagemSucesso"] = "Usuário ativo com sucesso!";
                }
                else
                {
                    TempData["MensagemSucesso"] = "Inativação realizada com sucesso!";
                }
                /*if (usuarioBanco.Cargo != PerfilEnum.Cliente)
                {
                    return RedirectToAction("Index", "Funcionario");
                }*/
                return RedirectToAction("Index", "Cliente", new { Id = "0" });
            }
            else
            {
                return RedirectToAction("Index");
            }

        }


        [HttpPost]
        public async Task<IActionResult> Cadastrar(UsuarioRegisterDto usuarioRegisterDto)

        {
            if (ModelState.IsValid)
            {
                if (!await _usuarioInterface.VerificaSeExisteUsuarioEEmail(usuarioRegisterDto))
                {

                    TempData["MensagemErro"] = "Já existe email/usuário cadastrado!";

                    return View(usuarioRegisterDto);


                }
                var usuario = await _usuarioInterface.Cadastrar(usuarioRegisterDto);
                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
                /*if (usuario.Cargo != PerfilEnum.Cliente)
                {
                    return RedirectToAction("Index", "Funcionario");
                }*/
                return RedirectToAction("Index", "Projeto");

            }
            else
            {
                return View(usuarioRegisterDto);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            var u = await _usuarioInterface.BuscarUsuarioPorId(id);

            ViewBag.Permissoes = new List<PermissoesModel>();

            foreach (PermissoesModel p in _dbContext.Permissoes )
            {
                
                if (!u.UsuarioPermissoes.Select(s => s.PermissaoId).Contains(p.Id))
                {
                    ((List<PermissoesModel>)ViewBag.Permissoes).Add(p);
                }
            }

            var user = _mapper.Map<UsuarioEditarDto>(u);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioEditarDto usuarioEditado)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioInterface.Editar(usuarioEditado);
                TempData["MensagemSucesso"] = "Edição realizada com sucesso!";

                /*if (usuario.Cargo != PerfilEnum.Cliente)
                {
                    return RedirectToAction("Index", "Funcionario");
                }*/
                return RedirectToAction("Index", "Cliente", new { Id = "0" });
            }
            else
            {
                return View(usuarioEditado);
            }

        }

        public IActionResult RemoverPermissao(UsuarioEditarDto usuarioEditar)
        {
            
            return View(usuarioEditar);
        }
    }
}
