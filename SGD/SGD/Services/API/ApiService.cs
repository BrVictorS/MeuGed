using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGD.Data;
using SGD.Dtos.Lote;
using SGD.Dtos.Response;
using SGD.Dtos.Verify;
using SGD.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SGD.Services.API
{
    public class ApiService : IApiInterface
    {
        private readonly DataDbContext _context;
        private readonly string _apiPath;
        private readonly HttpClient _httpClient;

        public ApiService(DataDbContext context, IHttpClientFactory factory)
        {
            _context = context;
            _apiPath = _context.Parametros.Where(p => p.Descricao == "APIDB").FirstOrDefault().Valor;
            _httpClient = factory.CreateClient("apiClient");

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
                return new ApiResponseDto("Falha na criação de metadados", true);
            }
        }

        public async Task<byte[]> GetImagem(string imagem)
        {
            using var http = new HttpClient();
            var response = await http.GetAsync($"{_apiPath}/Lote/GetImagem?imagem={imagem}");

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
                

                var chamada = await _httpClient.GetAsync($"api/Lote/{id}");

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
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                string json = System.Text.Json.JsonSerializer.Serialize(dto, options);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var chamada = await _httpClient.PutAsync(url, content);

                string resChamada = await chamada.Content.ReadAsStringAsync();

                ApiResponseDto resposta = System.Text.Json.JsonSerializer.Deserialize<ApiResponseDto>(resChamada);

                return resposta;
            }
            catch (Exception ex)
            {
                return new ApiResponseDto("Falha ao atualizar dados",true);
            }
        }

        public async Task<ApiResponseDto> InsereDocumento(InsereDocumentoDto documentoDto)
        {
            try
            {
                //supostamente a etiqueta ja foi protocolada
                bool exclusao = documentoDto.remover != null && documentoDto.remover == "1";
                var protocolo = _context.Protocolos.Include(d=>d.Documento).FirstOrDefault(x => x.Etiqueta == long.Parse(documentoDto.documento));

                if (protocolo == null)  //se nao nulo entao documento existe
                {
                    ProtocoloModel nProtocolo = new ProtocoloModel()
                    {
                        LoteId = int.Parse(documentoDto.LoteId),
                        Etiqueta = long.Parse(documentoDto.documento)
                        
                    };

                    protocolo = nProtocolo;
                    try
                    {
                        await _context.Protocolos.AddAsync(nProtocolo);
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        return new ApiResponseDto("Erro ao inserir protocolo", true);
                    }
                }
                else if (protocolo != null && !exclusao)
                {
                    return new ApiResponseDto($"Esta estiqueta já foi associada  no lote { _context.Lote.First(l=>l.Id == protocolo.LoteId).NumLote}",true);
                }



                var indexacao = _context.Indexacao.Include(d=> d.Documento).Include(c=> c.Documento.Protocolo).Any(p =>p.Documento.Protocolo.Etiqueta == protocolo.Etiqueta);

                if (indexacao)
                {
                    return new ApiResponseDto("Etiqueta já possui Indexação associada", true);
                }

                

                if (exclusao)
                {
                    documentoDto.documento = "";
                }

                //se protocolo existe e nao possui documentos associados entao pode inserir
                var salvaMongo = await ApiPut(documentoDto, "api/Lote/InsereDocumento"); // insere na api
                if (!salvaMongo.Status)
                {
                    return salvaMongo;
                }
                //se salvou na api entao salva o documento

                if (exclusao)
                {
                    _context.Remove(protocolo);
                    await _context.SaveChangesAsync();
                    return new ApiResponseDto("Documento excluido com sucesso!");
                }

                var novoDocumento = new DocumentoModel()
                {
                    ProtocoloId = protocolo.Id
                };


                var documentoSalvo =  _context.Documentos.Add(novoDocumento);

                await _context.SaveChangesAsync();
                return new ApiResponseDto("Documento salvo com sucesso!") { Dados =  documentoDto.documento};
            }
            catch(Exception ex)
            {
                documentoDto.documento = ""; //força inserir novo documento
                await ApiPut(documentoDto, "api/Lote/InsereDocumento");
                return new ApiResponseDto("Falha ao inserir documento", true);
            }
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

        
        public async Task<ApiResponseDto> ApiGet(string url)
        {
            

            var chamada = await _httpClient.GetAsync(url);

            string resChamada = await chamada.Content.ReadAsStringAsync();

            ApiResponseDto resposta = System.Text.Json.JsonSerializer.Deserialize<ApiResponseDto>(resChamada);


            return resposta;
        }

        public async Task<byte[]> GetImagemIndex(string documento)
        {
            using var http = new HttpClient();
            var response = await http.GetAsync($"{_apiPath}/Lote/GetImagemIndex?documento={documento}");

            if (!response.IsSuccessStatusCode)
                return new byte[0]; // ou lance exceção, conforme necessidade

            var imagemBytes = await response.Content.ReadAsByteArrayAsync();
            return imagemBytes;
        }

        [HttpPost]
        public async Task<ApiResponseDto> GetCaminhoImagem(string idImagem)
        {
            var chamada = await _httpClient.GetAsync($@"/api/Lote/GetCaminhoImagem?imagem={idImagem}");
            ApiResponseDto resposta = new ApiResponseDto();
            string resChamada = await chamada.Content.ReadAsStringAsync();
            try
            {
               resposta = System.Text.Json.JsonSerializer.Deserialize<ApiResponseDto>(resChamada);
            }
            catch
            {
                resposta = new ApiResponseDto() { Dados = resChamada };
            }            

            return resposta;
        }
    }
}
