using SGD.Dtos.Lote;
using SGD.Dtos.Response;
using SGD.Dtos.Verify;

namespace SGD.Services.API
{
    public interface IApiInterface
    {
        public Task<ApiResponseDto> EnviarLote(LoteApiDto loteApiDto);
        public Task<ServiceResponse<LoteApiDto>> GetLote(int id);
        public Task<byte[]> GetImagem(string imagem);
        public Task<ApiResponseDto> AtualizaImagem(AtualizaImagemDto imagem);

        public Task<ApiResponseDto> MoveImagem(MoveImagemDto imagem);
        public Task<ApiResponseDto> InsereDocumento(InsereDocumentoDto documentoDto);
        Task<ApiResponseDto> InsereImagem(string idLote, List<string> files, int posicao);
        Task<byte[]> GetImagemIndex(string documento);
        public Task<ApiResponseDto> GetCaminhoImagem(string idImagem);
    }
}
