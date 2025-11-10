using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using SGD.Data;
using SGD.Dtos.Index;
using SGD.Dtos.Response;
using SGD.Models;

namespace SGD.Services.Index
{
    public class IndexService : IIndexInterface
    {
        private readonly DataDbContext _context;
        public IndexService(DataDbContext context)
        {
            _context = context;
        }
        public Task<ServiceResponse<object>> GetDocumentosLote(int idLote)
        {
            throw new NotImplementedException();
        }

        public async  Task<ServiceResponse<object>> GetIndexacaoDocumento(string idDocumento)
        {
            int id_documento = 0;
            var documento = _context.Documentos
                .AsNoTracking()
                .Include(p => p.Protocolo)
                .FirstOrDefault(d => d.Protocolo.Etiqueta.ToString() == idDocumento);

            if (documento == null)
            {
                return new ServiceResponse<object>()
                {
                    Status = true,
                    Mensagem = "Documento não encontrado no protocolo."
                };
            }

            id_documento = documento.Id;

            List<IndexacaoDocumentoModel>? indexacaoDocumento = _context.Indexacao
                .Include(d => d.Documento)
                .Where(d => d.DocumentoId == id_documento)
                .ToList();

            if (indexacaoDocumento.Count == 0)
            {
                return new ServiceResponse<object>()
                {
                    Status = true
                };
            }
            var indexacaoDto = new IndexacaoDocumentoDto();
            var doc = indexacaoDocumento.FirstOrDefault();
            indexacaoDto.idDocumento = doc.Documento.Id.ToString();
            indexacaoDto.idTipoDoc = doc.Documento.TipoDocId.ToString();
            indexacaoDto.idLote = doc.LoteId.ToString();           
            foreach (var indice in indexacaoDocumento)
            {
                indexacaoDto.metadados.Add(
                        new MetadadoDto()
                        {
                            id = indice.MetadadoTipoDocId.ToString(),
                            valor = indice.Valor
                        }
                    
                    );
            }

            return new ServiceResponse<object>()
            {
                Status = true,
                Dados = indexacaoDto
            };

        }

        public Task<ServiceResponse<object>> GetMetadadosTipoDoc(int tipoDoc)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<IndexTipoDocDto>>> GetTiposDocumentoLote(string idLote)
        {
            var response = new ServiceResponse<List<IndexTipoDocDto>>();
            var docs = _context.TipoDocumental.AsNoTracking().Select(x => 
                    new IndexTipoDocDto() {
                            Id = x.Id,
                            Name = x.Name,
                            Metadados = x.Metadados.Select(m => new IndexMetadadoDto() {
                                Id = m.Id,
                                Nome = m.Metadado.Nome
                            }).ToList()

                    }).ToList();
            response.Dados = docs;
            return response;
        }

        public async Task<ServiceResponse<object>> SalvarIndexaoDocumento(IndexacaoDocumentoDto indexacaoDocumentoDto)
        {
            try
            {

                List<IndexacaoDocumentoModel> documentos = new List<IndexacaoDocumentoModel>();

                //verifica se o documento foi criado ao etiquetar manualmente na verificacao
                var documentoExistente = _context.Documentos.Include(d => d.Protocolo).FirstOrDefault(s => s.Protocolo.Etiqueta.ToString() == indexacaoDocumentoDto.idDocumento.ToString());

                if (documentoExistente == null)
                {
                    return new ServiceResponse<object>()
                    {
                        Status = false,
                        Mensagem = "Não há documento para esta etiqueta"
                    };

                }

                //Remove indexacoes anteriores para este documento
                List<IndexacaoDocumentoModel> indexExistente = _context.Indexacao.Include(d => d.Documento).Where(s => s.Documento.Id == int.Parse(documentoExistente.Id.ToString())).ToList();


                if (indexExistente != null)
                {
                    _context.Indexacao.RemoveRange(indexExistente);
                }

                // atualiza o tipo do documento para o tipo indexado
                documentoExistente.TipoDocId = int.Parse(indexacaoDocumentoDto.idTipoDoc);
                


                foreach (var doc in indexacaoDocumentoDto.metadados)
                {
                    if (!string.IsNullOrEmpty(doc.valor))
                    {
                        var id_documento = await _context.Documentos.Include(a => a.Protocolo)
                            .FirstOrDefaultAsync(a => a.Protocolo.Etiqueta == long.Parse(indexacaoDocumentoDto.idDocumento.ToString()));

                        IndexacaoDocumentoModel documento = new IndexacaoDocumentoModel()
                        {
                            DocumentoId = id_documento.Id,
                            LoteId = int.Parse(indexacaoDocumentoDto.idLote),
                            MetadadoTipoDocId = int.Parse(doc.id),
                            Valor = doc.valor
                        };
                        documentos.Add(documento);
                    }
                }

                if (documentos.Count > 0)
                {
                    await _context.Indexacao.AddRangeAsync(documentos);
                    await _context.SaveChangesAsync();
                }
                return new ServiceResponse<object>()
                {
                    Status = true,
                    Mensagem = "Indexação do documento salva com sucesso."
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse<object>
                {
                    Status = false,
                    Mensagem = "Erro ao salvar indexação do documento."
                };
            }

                    
        }
    }
}
