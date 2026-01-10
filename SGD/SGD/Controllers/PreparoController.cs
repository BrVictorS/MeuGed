using AutoMapper;
using SGD.Data;
using SGD.Dtos.Preparo;
using SGD.Dtos.Usuarios;
using SGD.Enums;
using SGD.Models.ViewModels;
using SGD.Services.Fluxo;
using SGD.Services.Preparo;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SGD.Controllers
{
    public class PreparoController : Controller
    {
        private readonly DataDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFluxoInterface _fluxo;

        public PreparoController(DataDbContext context,IMapper mapper, IPreparoInterface preparo, IFluxoInterface fluxo) 
        {
            _context = context;
            _mapper = mapper;
            _fluxo = fluxo;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            List<UsuarioDto> usuarioDtos = _mapper.Map<List<UsuarioDto>>(usuarios);
            PreparoViewModel viewModel = new PreparoViewModel();
            viewModel.Usuarios = usuarioDtos;

            return View(viewModel);
        }

        public IActionResult ImpressaoEtiqueta()
        {
            List<EtiquetaCodBarrasDto> etq = new List<EtiquetaCodBarrasDto>();

            // Usando 'for' para ficar mais limpo, mas o while também funcionaria
            for (int i = 1; i <= 126; i++)
            {
                etq.Add(new EtiquetaCodBarrasDto()
                {
                    // "D6" formata o inteiro com 6 dígitos, preenchendo com zeros à esquerda.
                    // Ex: 1 vira "000001", 126 vira "000126".
                    // O formato ITF EXIGE quantidade par de números.
                    CodigoBarras = i.ToString("D6")
                });
            }

            return View("ImpressaoEtiqueta", etq);
        }

        [HttpPost]
        public async Task<IActionResult> SalvarPreparoAsync(string jsonLotes)
        {
            var lista = JsonSerializer.Deserialize<List<LoteUsuarioDto>>(jsonLotes);

            //new List<LoteUsuarioDto> ();


           /* int indx = 5;

            while (indx <= 1000)
            {
                lista.Add(new LoteUsuarioDto() { IdUsuario=1,NumeroLote=indx.ToString().PadLeft(6,'0')});

                indx++;
            }
*/

            foreach (var item in lista)
            {
                var response = await _fluxo.SalvarFluxo(item.IdUsuario, int.Parse(HttpContext.Session.GetString("idProjeto")), item.NumeroLote, 2, null);
                if (!response.Status)
                {
                    TempData["erro"] += response.Mensagem + Environment.NewLine;
                }
                else
                {
                    TempData["ok"] += $"Preparo do lote {item.NumeroLote} realizado"+Environment.NewLine;
                }

            }


            return RedirectToAction("Index");
        }
    }
}
