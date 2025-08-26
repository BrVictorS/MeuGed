using GedDb.Data;
using GedDb.Dto;
using GedDb.Entities;
using GedDbApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace GedDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoteController : ControllerBase
    {
        private readonly IMongoCollection<Lote>? _lote;    
        private readonly MongoDbService _database;

        public LoteController(MongoDbService mongoDbService, MongoDbService dbService)
        {
            _lote = mongoDbService.Database.GetCollection<Lote>("lote");
            _database = dbService;
        }


        [HttpGet]
        public async Task<IEnumerable<Lote>> Get()
        {
            return await _lote.Find(FilterDefinition<Lote>.Empty).ToListAsync();
        }


        [HttpGet("GetImagem")] 
        public async Task<IActionResult> GetImagem(string imagem)
        {
            var filter = Builders<Lote>.Filter.Eq("imagens._id", imagem);
            var projection = Builders<Lote>.Projection
                .Include(l => l.idLote)
                .Include(l => l.NumLote)
                .ElemMatch(l => l.Imagens, i => i.Id == imagem);

            var result = await _lote.Find(filter)
                .Project<Lote>(projection)
                .FirstOrDefaultAsync();

            if (result == null)
                return NotFound(new { status = false, msg = "Lote não encontrado" });

            // Procura a imagem dentro do lote
            var imgPath = Path.Combine(
                _database._arquivos,
                result.idLote,
                result.Imagens.First().Caminho
                );

            

            if (string.IsNullOrEmpty(imgPath) || !System.IO.File.Exists(imgPath))
                return NotFound(new { status = false, msg = "Imagem não encontrada para o lote" });

            // Lê o arquivo como bytes
            var imgBytes = await System.IO.File.ReadAllBytesAsync(imgPath);

            // Define o content type de acordo com a extensão
            var ext = Path.GetExtension(imgPath).ToLower();
            string contentType = ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".tif" or ".tiff" => "image/tiff", // navegadores podem não suportar
                _ => "application/octet-stream"
            };

            return File(imgBytes, contentType);
        }

        [HttpGet("{idLote}")]
        public async Task<ActionResult<Lote>> GetById(string idLote)
        {
            var filter = Builders<Lote>.Filter.Eq(x => x.idLote, idLote);
            var lote = _lote.Find(filter).FirstOrDefault();
            return lote is not null ? Ok(lote) : NotFound(new { status = false, msg = "Lote não encontrado" });
        }


        [HttpPost]
        public async Task<ActionResult> Create(Lote lote)
        {
            var filter = Builders<Lote>.Filter.Eq(x => x.idLote, lote.idLote);
            var lt = _lote.Find(filter).FirstOrDefault();
            if (lt is not null)
            {
                return StatusCode(500, new { status = false, msg = "lote ja existe" });
            }


            foreach (Imagem img in lote.Imagens)
            {
                img.Id = ObjectId.GenerateNewId().ToString();
            }

            await _lote.InsertOneAsync(lote);

            return StatusCode(200, new { status = true, msg = "Lote criado com sucesso" });
            //return CreatedAtAction(nameof(GetById), new { idLote = lote.idLote,status = true, msg = "Lote criado com sucesso" }, lote);
        }

        //[HttpPut]

        //public async Task<ActionResult> Update(Lote lote)
        //{
        //    var filter = Builders<Lote>.Filter.Eq(x => x.id, lote.id);
        //    //var update = Builders<Customer>.Update
        //    //    .Set(x => x.CustomerName, customer.CustomerName)
        //    //    .Set(x => x.Email, customer.Email);
        //    //await _customers.UpdateOneAsync(filter, update);

        //    await _lote.ReplaceOneAsync(filter, lote);
        //    return Ok();
        //}

        [HttpPut("InsereDocumento")]
        public async Task<ActionResult> InsereDocumento([FromBody] InsereDocumentoDto dto)
        {
            try
            {
                var filter = Builders<Lote>.Filter.ElemMatch(x => x.Imagens, img => img.Id == dto.id);

                var update = Builders<Lote>.Update
                    .Set("Imagens.$.DocumentoId", dto.documento);

                await _lote.UpdateOneAsync(filter, update);
            }
            catch (Exception ex) 
            {
                return BadRequest(new {status = false, msg = "Erro ao inserir documento" });            
            }

            return Ok(new { status = true, msg = "Documento inserido com sucesso" });
        }

        [HttpPut("imagem")]
        public async Task<ActionResult> SetSituacaoImagem([FromBody] SituacaoImagemUpdateDto dto)
        {
            if (ModelState.IsValid)
            {                
                var filter = Builders<Lote>.Filter.ElemMatch(x => x.Imagens, img => img.Id == dto.Imagem);

                var update = Builders<Lote>.Update
                    .Set("Imagens.$.Situacao", dto.Situacao);

                await _lote.UpdateOneAsync(filter, update);
                return Ok(new { status = true, msg = "imagem atualizada com sucesso" });
            }
            else
            {
                return BadRequest(new { status = false, msg = "falha ao atualizar imagem" });
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var filter = Builders<Lote>.Filter.Eq(x => x.idLote, id);
            await _lote.DeleteOneAsync(filter);
            return Ok();
        }

        [HttpPut("MoveImagem")]
        public async Task<IActionResult> MoveImagem([FromBody] MoveImagemDto dto)
        {
            // 1. Buscar o lote que contém a imagem
            var filter = Builders<Lote>.Filter.ElemMatch(l => l.Imagens, i => i.Id == dto.id);
            var lote = await _lote.Find(filter).FirstOrDefaultAsync();

            if (lote == null)
                return NotFound(new { status = false, msg = "Imagem não encontrada em nenhum lote" });

            // 2. Localizar a imagem
            var imagem = lote.Imagens.FirstOrDefault(i => i.Id == dto.id);
            if (imagem == null)
                return NotFound(new { status = false, msg = "Imagem não encontrada" });

            // 3. Remover e reinserir na nova posição
            lote.Imagens.Remove(imagem);

            if (dto.posicao < 0) dto.posicao = 0;
            if (dto.posicao > lote.Imagens.Count) dto.posicao = lote.Imagens.Count;

            lote.Imagens.Insert(dto.posicao, imagem);

            // 4. Atualizar o documento no Mongo
            var update = Builders<Lote>.Update.Set(l => l.Imagens, lote.Imagens);
            await _lote.UpdateOneAsync(filter, update);

            return Ok( new { status = true, msg = "Imagem movida com sucesso" });
        }

        [HttpPut("InsereImagem")]
        public async Task<IActionResult> InsereImagem([FromBody] InsereImagemDto dto)
        {
            try
            {
                var filter = Builders<Lote>.Filter.Eq(x => x.idLote, dto.idLote);
                var lote = _lote.Find(filter).FirstOrDefault();
                int newP = dto.posicao+1;
                if (lote == null)
                    return NotFound(new { status = false, msg = "Imagem não encontrada em nenhum lote" });

                if (dto.posicao < 0) dto.posicao = 0;
                if (dto.posicao > lote.Imagens.Count) dto.posicao = lote.Imagens.Count;

                foreach (var img in dto.files)
                {
                    Imagem imgm = new Imagem();
                    imgm.Id = ObjectId.GenerateNewId().ToString();
                    imgm.Caminho = Path.GetFileName(img);
                    lote.Imagens.Insert(newP, imgm);
                    newP++;
                }

                // 4. Atualizar o documento no Mongo
                var update = Builders<Lote>.Update.Set(l => l.Imagens, lote.Imagens);
                await _lote.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                return BadRequest(new {status = false, msg = ex.Message});
            }
            return Ok(new { status = true, msg = "Imagem(s) inserida(s) com sucesso"! });
        }
    }
}
