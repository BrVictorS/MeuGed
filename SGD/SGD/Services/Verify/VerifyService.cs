using SGD.Dtos.Lote;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using SGD.Data;
using SGD.Dtos.Response;

namespace SGD.Services.Verify
{
    public class VerifyService
    {
        private readonly DataDbContext _context;
        private readonly string _apiPath;

        public VerifyService(DataDbContext context)
        {
            _context = context;
            _apiPath = _context.Parametros.Where(p => p.Descricao == "APIDB").FirstOrDefault().Valor;
        }
    }
}
