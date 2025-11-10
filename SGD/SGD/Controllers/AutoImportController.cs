using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using SGD.Dtos.Lote;
using SGD.Dtos.Processolote;
using SGD.Dtos.Response;
using SGD.Services.API;
using SGD.Services.Barcode;
using SGD.Services.Fluxo;
using SGD.Services.SelecionarLote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Controllers
{
    public class AutoImportController : BaseController
    {
        private readonly IBarcodeServiceInterface _barcodeService;
        private readonly IFluxoInterface _fluxo;
        private readonly ISelecionalote _selecionaLote;
        private readonly IApiInterface _api;

        public AutoImportController(IBarcodeServiceInterface barcodeService, IFluxoInterface fluxoInterface,ISelecionalote selecionalote, IApiInterface api)
        {
            _barcodeService = barcodeService;
            _fluxo = fluxoInterface;
            _selecionaLote = selecionalote;
            _api = api;
        }
        public IActionResult Index()
        {
            /*  _barcodeService.ConfigurarCodigoBarras();
              string imagem = (@"C:\Users\victor\Downloads\0001.TIF");
              var codigo = _barcodeService.CapturarCodigoBarras(imagem);*/
            List<SelecionarLoteDto> dtoList = new List<SelecionarLoteDto>();
            dtoList = _selecionaLote.GetLotesFila(4).Result.Dados;
            return View(dtoList);
        }

        public async Task<IActionResult> QuebraLote([FromBody]string idLote)
        {
            var lote =  await _api.GetLote(int.Parse(idLote));
            //_barcodeService.QuebraLote((LoteApiDto)lote.Dados);
            //await EnfileirarLote(lote.Dados);



            return View();
        }

        public async Task<IActionResult> EnfileirarLote([FromBody] LoteApiDto lote)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };

                await using var connection = await factory.CreateConnectionAsync();
                await using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: "fila_lotes",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var json = JsonConvert.SerializeObject(lote);
                var body = Encoding.UTF8.GetBytes(json);

                var props = new BasicProperties
                {
                    DeliveryMode = DeliveryModes.Persistent // garante persistência
                };

                await channel.BasicPublishAsync<BasicProperties>(
                    exchange: "",
                    routingKey: "fila_lotes",
                    mandatory: false,
                    basicProperties: props,
                    body: body,
                    cancellationToken: default);

                return Ok("Lote enfileirado com sucesso.");
            }
            catch (BrokerUnreachableException)
            {
                return StatusCode(500, "Não foi possível conectar ao RabbitMQ.");
            }
        }

        public async Task<IActionResult> GetImagens([FromBody] string idLote)
        {
            var lote =  await _api.GetLote(int.Parse(idLote));

            if (!lote.Status)
            {
                return NotFound("Lote não encontrado.");
            }

            var imagens = new List<ImagemImportDto>();

            foreach (var imagem in lote.Dados.imagens)
            {
                imagens.Add(new ImagemImportDto
                {
                    idImagem = imagem.id,
                    imagem = imagem.caminho,
                    protocolo = imagem.documentoId
                });
            }

            return Ok(imagens);
        }

        [HttpPost]
        public async Task<IActionResult> QuebraImagem(string idImagem,string loteId)
        {
           
            var caminhoImagem = await _api.GetCaminhoImagem(idImagem);

            if (!caminhoImagem.Status)
            {
                return Json(caminhoImagem);
            }

            var codBarras = _barcodeService.CapturarCodigoBarras(caminhoImagem.Dados);

            if (string.IsNullOrEmpty(codBarras))
            {
                return await RespostaJson(new ApiResponseDto(codBarras));
            }


            var response = await _api.InsereDocumento(new Dtos.Verify.InsereDocumentoDto
            {
                documento = codBarras,
                id = idImagem,
                LoteId = loteId,

            });

            return await RespostaJson(response);            
            
        }


        [HttpPost]
        public async Task<IActionResult> InsereFluxo([FromBody] int idLote)
        {
            var idUsuario = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        
            var Fluxo = await _fluxo.InsereFluxo(idUsuario, int.Parse(HttpContext.Session.GetString("idProjeto")),idLote,4);            
            return await RespostaJson(Fluxo);
        }



        [HttpPost]
        public async Task<IActionResult> FinalizaFluxo(string idLote,string idFLuxo)
        {
            var idUsuario = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);            
            var response = await _fluxo.FinalizaFluxo(idUsuario, int.Parse(idFLuxo), "");
            return await RespostaJson(response);
        }

    }
}
