using Microsoft.AspNetCore.Mvc;
using SGD.Dtos.Response;
using SGD.Dtos.TipoDocumento;
using SGD.Models;

namespace SGD.Services.TipoDocumento
{
    public interface ITipoDocumentoInterface
    {
        public ServiceResponse<List<TipoDocumentalModel>> BuscarTipoDocumento();
        public List<MetadadosDocumentoDto> BuscarMetadados(int? tipoDoc);
        public Task<ServiceResponse<string>> NovoMetadado(string metadado);
        public Task SalvarNovoTipoDoc(TipoDocumentoDto tipodoc);
        public TipoDocumentoDto GetTipoDocById(int id);
        public Task<ServiceResponse<List<TipoDocumentalModel>>> Editar(TipoDocumentoDto documentoDto);
    }
}
