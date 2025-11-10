
using Microsoft.AspNetCore.Mvc;
using SGD.Dtos.Arquivo;
using SGD.Dtos.Lote;
using SGD.Dtos.Response;
using SGD.Dtos.Verify;

namespace SGD.Services.Arquivo
{
    public interface IArquivoInterface
    {        
        Task<ServiceResponse<string>> EnviaLote(IFormFileCollection files,string idLote);
        Task<byte[]> GetImagem(string imagem);
        Task<byte[]> GetImagemIndex(string documento);
        public Task<ServiceResponse<string>> InsereImagem(InsereImagemDto insereImagemDto);
        public void MoveLote();
    }
}
