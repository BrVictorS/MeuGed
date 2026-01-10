using Microsoft.AspNetCore.Mvc;
using SGD.Dtos.Response;
using SGD.Dtos.TipoDocumento;
using SGD.Services.TipoDocumento;

namespace SGD.Controllers
{
    public class TipoDocumentoController : Controller
    {
        private readonly ITipoDocumentoInterface _tipoDocumento;
        public TipoDocumentoController(ITipoDocumentoInterface tipoDocumentoInterface)
        {
            _tipoDocumento = tipoDocumentoInterface;
        }

        public IActionResult Index()
        {
            var docs = _tipoDocumento.BuscarTipoDocumento();
            
            return View(docs);
        }

        public IActionResult Cadastrar(int tipoDoc) 
        {            
            TipoDocumentoDto novo = new TipoDocumentoDto();
            novo.metadadosDocumentos = _tipoDocumento.BuscarMetadados(tipoDoc);

            return View("Cadastrar",novo);
        }


        [HttpPost]

        public async Task<IActionResult> Editar(TipoDocumentoDto documentoDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _tipoDocumento.Editar(documentoDto);
                
                return View("Index",response);
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Novo(TipoDocumentoDto tipodoc)
        {
            if (ModelState.IsValid)
            {

               await _tipoDocumento.SalvarNovoTipoDoc(tipodoc);
            }
            else
            {
                return View("Cadastrar", tipodoc);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            TipoDocumentoDto doc = _tipoDocumento.GetTipoDocById(id);
            return View("Cadastrar",doc);
        }


        public async Task<IActionResult> NovoMetadado([FromBody] string metadado)
        {
            var salvar = await _tipoDocumento.NovoMetadado(metadado);

            if (salvar == null)
                return Json(new { sucesso = false });

            return Json(new
            {
                sucesso = true,
                metadado = new { name = metadado,id = salvar.Dados}
            });
        }
    }
}
