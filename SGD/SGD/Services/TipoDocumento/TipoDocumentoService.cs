using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGD.Data;
using SGD.Dtos.Response;
using SGD.Dtos.TipoDocumento;
using SGD.Models;
using System.Collections.Generic;

namespace SGD.Services.TipoDocumento
{
    public class TipoDocumentoService : ITipoDocumentoInterface
    {
        private readonly DataDbContext _context;
        public TipoDocumentoService(DataDbContext context)
        {
            _context = context;
        }

        public ServiceResponse<List<TipoDocumentalModel>> BuscarTipoDocumento()
        {
            var docs = _context.TipoDocumental.ToList();
            ServiceResponse<List<TipoDocumentalModel>> response = new ServiceResponse<List<TipoDocumentalModel>>()
            {
                Dados = docs
            };

            return response;
        }

        public List<MetadadosDocumentoDto> BuscarMetadados(int? tipoDoc)
        {
            if (tipoDoc == null)
            {
                tipoDoc = 999;
            }

            var dados =  _context.Metadados
                                                .Select(m => new MetadadosDocumentoDto
                                                {
                                                    Id = m.Id,
                                                    Nome = m.Nome,
                                                    Selecionado = m.MetadadosTipoDoc
                                                        .Any(r => r.TipoDocumentalId == tipoDoc) ? 1 : 0
                                                })
                                                .ToList();
            //var ss = from MetadadosModel in _context.Metadados join 
            return dados;
        }


        public async Task<ServiceResponse<string>> NovoMetadado(string metadado)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                MetadadosModel metadadoModel = new MetadadosModel();
                metadadoModel.Nome = metadado;
                var salvar = _context.Metadados.Add(metadadoModel);
                await _context.SaveChangesAsync();
            }
            catch
            {
                response.Status = false;
                response.Mensagem = "Erro ao salvar Metadado";
            }            
            return response;
        }

        public async Task SalvarNovoTipoDoc(TipoDocumentoDto tipodoc)
        {
            TipoDocumentalModel tip = new TipoDocumentalModel();
            tip.Name = tipodoc.Name;
            tip.Metadados = new List<MetadadosTipoDocModel>();

            foreach (int mtd in tipodoc.MetadadosSelecionados)
            {
                tip.Metadados.Add(new MetadadosTipoDocModel() { MetadadoId = mtd, TipoDocumentalId = tip.Id });
            }

            _context.TipoDocumental.Add(tip);

            await _context.SaveChangesAsync();            
        }

        public TipoDocumentoDto GetTipoDocById(int id)
        {
            TipoDocumentoDto doc = new TipoDocumentoDto();
            var cdoc = _context.TipoDocumental.Include( p => p.Metadados).Where(t => t.Id == id).FirstOrDefault();
            doc.Name = cdoc.Name;
            doc.Id = cdoc.Id ;

            var mtd = _context.Metadados;

            foreach (var m in mtd)
            {
                MetadadosDocumentoDto met = new MetadadosDocumentoDto();
                if (cdoc.Metadados.Select(c => c.MetadadoId).ToList().Contains(m.Id))
                {
                    met.Selecionado = 1;
                }
                else
                {
                    met.Selecionado = 0;
                }
                met.Nome = m.Nome;
                met.Id = m.Id;
                
                doc.metadadosDocumentos.Add(met);
            }


            return doc;
        }

        public async Task<ServiceResponse<List<TipoDocumentalModel>>> Editar(TipoDocumentoDto documentoDto)
        {
            ServiceResponse<List<TipoDocumentalModel>> response = new ServiceResponse<List<TipoDocumentalModel>>();
            try
            {
                

                var docs = await _context.TipoDocumental
                    .Include(d => d.Metadados)
                    .FirstOrDefaultAsync(x => x.Id == documentoDto.Id);

                if (docs == null)
                {
                    response.Status = false;
                    response.Mensagem = "Tipo documental não encontrado.";
                    return response;
                }

                // 1. Remover metadados que não estão mais selecionados
                var mtdDel = docs.Metadados
                    .Select(dm => dm.MetadadoId)
                    .Where(m => !documentoDto.MetadadosSelecionados.Contains(m))
                    .ToList();

                foreach (var mtdId in mtdDel)
                {
                    var del = docs.Metadados.FirstOrDefault(m => m.MetadadoId == mtdId);
                    if (del != null)
                    {
                        docs.Metadados.Remove(del);
                    }
                }

                // 2. Adicionar novos metadados que não existiam antes
                foreach (var mtdId in documentoDto.MetadadosSelecionados)
                {
                    bool existe = docs.Metadados
                        .Any(m => m.TipoDocumentalId == documentoDto.Id && m.MetadadoId == mtdId);

                    if (!existe)
                    {
                        var novoMtd = new MetadadosTipoDocModel
                        {
                            TipoDocumentalId = documentoDto.Id,
                            MetadadoId = mtdId
                        };
                        docs.Metadados.Add(novoMtd);
                    }
                }

                await _context.SaveChangesAsync();

                response.Status = true;
                response.Mensagem = "Salvo com sucesso";

                
            }
            catch (Exception ex)
            {
                response.Mensagem = "Erro ao salvar";
            }

            response.Dados = BuscarTipoDocumento().Dados;
            return response;
        }

    }
}
