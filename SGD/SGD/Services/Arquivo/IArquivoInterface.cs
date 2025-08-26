
using Microsoft.AspNetCore.Mvc;
using SGD.Dtos.Arquivo;
using SGD.Dtos.Lote;
using SGD.Dtos.Response;
using SGD.Dtos.Verify;

namespace SGD.Services.Arquivo
{
    public interface IArquivoInterface
    {        
        Task<ServiceResponse<bool>> EnviaLote(IFormFileCollection files,string idLote);
        Task<byte[]> GetImagem(string imagem);
        public Task<ServiceResponse<bool>> InsereImagem(InsereImagemDto insereImagemDto);
        public void MoveLote();
    }
}
