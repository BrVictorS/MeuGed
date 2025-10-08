using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGD.Data;
using SGD.Dtos.Lote;
using SGD.Models;
using SGD.Services.Arquivo;
using SGD.Services.Fluxo;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using SGD.Dtos.Response;
using SGD.Services.API;
using ImageMagick;
using SGD.Dtos.Arquivo;
using Microsoft.EntityFrameworkCore;
using SGD.Dtos.Verify;

namespace SGD.Services.Scan
{
    public class ArquivoService : IArquivoInterface
    {
        private readonly DataDbContext _context;        
        private readonly IApiInterface _apiService;
        private readonly string _arquivosPath;

        public ArquivoService(DataDbContext context, IApiInterface api)
        {
            _context = context;            
            _apiService = api;
            _arquivosPath = _context.Parametros.FirstOrDefault(x => x.Descricao == "LOTES").Valor;
        }

        public async Task<ServiceResponse<bool>> EnviaLote(IFormFileCollection files, string idLote)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            
            LoteApiDto loteApiDto = new LoteApiDto();
            loteApiDto.idLote = idLote;
            loteApiDto.numLote = _context.Lote.FirstOrDefaultAsync(x=>x.Id == int.Parse(idLote)).Result.NumLote;
            loteApiDto.imagens = files.Select(i => new ImagemLoteDto() { caminho = Path.GetFileName(i.FileName), situacao = "OK" }).ToList();

            var api = await _apiService.EnviarLote(loteApiDto);// await EnviarLoteAPI(loteApiDto);


            if (api.status)
            {
                response = await EnviaArquivos(files,idLote);
            }
            else
            {
                response.Mensagem = api.msg;
                response.Status = api.status;                
            }
            
            return response;
        }
        
        public void MoveLote()
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetImagem(string imagem)
        {
            var imgbyte = await _apiService.GetImagem(imagem);

            using (var img = new MagickImage(imgbyte))
            {
                img.Format = MagickFormat.Png;
                using var stream = new MemoryStream();
                img.Write(stream);
                return stream.ToArray();
            }
        }

        public async Task<ServiceResponse<bool>> InsereImagem(InsereImagemDto dto)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            response = await _apiService.InsereImagem(dto.idLote,dto.files.Select(x => x.FileName).ToList(),dto.posicao);
            if (!response.Status)
            {
                return response;
            }
            else
            {
                return await EnviaArquivos(dto.files, dto.idLote);
            }
            

        }

        private async Task<ServiceResponse<bool>> EnviaArquivos(IFormFileCollection files, string idLote)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                string PastaLote = Path.Combine(_arquivosPath, idLote);
                if (!Directory.Exists(PastaLote))
                    Directory.CreateDirectory(PastaLote);

                foreach (var arquivo in files)
                {
                    if (arquivo.Length > 0)
                    {
                        var caminhoFinal = Path.Combine(PastaLote, Path.GetFileName(arquivo.FileName));
                        using (var stream = new FileStream(caminhoFinal, FileMode.Create))
                        {
                            await arquivo.CopyToAsync(stream);
                        }
                    }
                }

                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
            }

            return response;
        }

        public async Task<byte[]> GetImagemIndex(string documento)
        {
            var imgbyte = await _apiService.GetImagemIndex(documento);

            using (var img = new MagickImage(imgbyte))
            {
                img.Format = MagickFormat.Png;
                using var stream = new MemoryStream();
                img.Write(stream);
                return stream.ToArray();
            }
        }
    }
}
