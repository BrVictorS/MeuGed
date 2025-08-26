using SGD.Dtos.Lote;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SGD.Data;
using SGD.Dtos.Response;
using System.Net.Http;
using SGD.Dtos.Verify;

namespace SGD.Services.API
{
    public class ApiService : IApiInterface
    {
        private readonly DataDbContext _context;
        private readonly string _apiPath;

        public ApiService(DataDbContext context)
        {
            _context = context;
            _apiPath = _context.Parametros.Where(p => p.Descricao == "APIDB").FirstOrDefault().Valor;
        }

        public async Task<ApiResponseDto> AtualizaImagem(AtualizaImagemDto imagem)
        {
            return await ApiPut(imagem, "api/Lote/imagem");           
        }

        
        public async Task<ApiResponseDto> EnviarLote(LoteApiDto loteApiDto)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiPath);

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Converters = { new JsonStringEnumConverter() }
                    };

                    string json = System.Text.Json.JsonSerializer.Serialize(loteApiDto, options);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");


                    var chamada = await httpClient.PostAsync("api/Lote", content);

                    string resChamada = await chamada.Content.ReadAsStringAsync();

                    ApiResponseDto resposta = System.Text.Json.JsonSerializer.Deserialize<ApiResponseDto>(resChamada);

                    return resposta;
                }
            }
            catch (Exception ex)
            {
                return new ApiResponseDto() { status = false, msg = "Falha na criação de metadados" };
            }
        }

        public async Task<byte[]> GetImagem(string imagem)
        {
            using var http = new HttpClient();
            var response = await http.GetAsync($"{_apiPath}api/Lote/GetImagem?imagem={imagem}");

            if (!response.IsSuccessStatusCode)
                return new byte[0]; // ou lance exceção, conforme necessidade

            var imagemBytes = await response.Content.ReadAsByteArrayAsync();
            return imagemBytes;
        }

        public async Task<ServiceResponse<LoteApiDto>> GetLote(int id)
        {
            ServiceResponse<LoteApiDto> response = new ServiceResponse<LoteApiDto>();
            try
            {                
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiPath);
                                         
                    var chamada = await httpClient.GetAsync($"{_apiPath}api/Lote/{id}");

                    string resChamada = await chamada.Content.ReadAsStringAsync();

                    LoteApiDto resposta = System.Text.Json.JsonSerializer.Deserialize<LoteApiDto>(resChamada);

                    if (resposta.idLote == null)
                    {
                        response.Status = false;
                        response.Mensagem = "Metadados não encontrados para o lote";
                    }

                    response.Dados = resposta;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Mensagem = ex.Message;
            }
            return response;
        }

        public async Task<ApiResponseDto> MoveImagem(MoveImagemDto imagem)
        {
            return await ApiPut(imagem, "api/Lote/MoveImagem");
        }

        public async Task<ApiResponseDto> ApiPut(object dto,string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiPath);

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Converters = { new JsonStringEnumConverter() }
                    };

                    string json = System.Text.Json.JsonSerializer.Serialize(dto, options);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var chamada = await httpClient.PutAsync(url, content);

                    string resChamada = await chamada.Content.ReadAsStringAsync();

                    ApiResponseDto resposta = System.Text.Json.JsonSerializer.Deserialize<ApiResponseDto>(resChamada);

                    return resposta;
                }
            }
            catch (Exception ex)
            {
                return new ApiResponseDto() { status = false, msg = "Falha ao atualizar dados" };
            }
        }

        public async Task<ApiResponseDto> InsereDocumento(InsereDocumentoDto documentoDto)
        {
            return await ApiPut(documentoDto, "api/Lote/InsereDocumento");
        }

        public async Task<ApiResponseDto> InsereImagem(string idLote,List<string> files, int posicao)
        {
            var obj = new {
                idLote = idLote,
                files = files,
                posicao = posicao
            
            };

            return await ApiPut(obj, "api/Lote/InsereImagem");
        }
    }
}
