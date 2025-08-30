using SGD.Data;
using SGD.Enums;
using SGD.Models;
using Microsoft.EntityFrameworkCore;
using SGD.Dtos.Response;

namespace SGD.Services.Fluxo
{
    public class FluxoService : IFluxoInterface
    {
        private readonly DataDbContext _context;
        public FluxoService(DataDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<string>> FinalizaFluxo(int idUsuario,int idFluxo, string observacao)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                var fluxo = await _context.FluxoLote.FindAsync(idFluxo);
                fluxo.DtFim = DateTime.Now;

                response.Dados = fluxo.DtFim?.ToString("dd/MM/yyyy HH:mm:ss");
                response.Mensagem = "Finalizado com suceso!";
                await _context.SaveChangesAsync();
            }
            catch
            {
                response.Status = false;
                response.Mensagem = "Erro ao finalizar fluxo";
            }

            return response;
        }

        public async Task<int?> GetLoteByNum(string numLote, string idProjeto)
        {
            int projId = int.Parse(idProjeto);

            return await _context.Lote
                .Where(l => l.ProjetoId == projId && l.NumLote == numLote)
                .Select(l => (int?)l.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceResponse<int>> InsereFluxo(int idUsuario,int idProjeto, int idLote, int idSituacao)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();            
            try
            {
                var lote = await _context.Lote
                .Include(l => l.Fluxos)
                .FirstOrDefaultAsync(l => l.Id == idLote);

                if (lote == null)
                {
                    response.Status = false;
                    response.Mensagem = "Lote não existe";
                }
                else
                {
                    if (lote.Fluxos.LastOrDefault() != null && idSituacao > 1 )
                    {
                        
                    }

                    Tuple<bool, int> valido = await ValidaFluxo(idLote, idSituacao);
                    if (!valido.Item1 && valido.Item2 == 0)
                    {
                        response.Status = false;
                        response.Mensagem = $"O lote {lote.NumLote} não está na fila selecionada!";
                        return response;
                    }
                    else if (valido.Item1 && valido.Item2 > 0)
                    {
                        response.Dados = valido.Item2;
                        return response;
                    }


                    var fluxo = new FluxoModel()
                    {
                        SituacaoId = idSituacao,
                        UsuarioId = idUsuario,
                        LoteId = lote.Id
                    };

                    lote.Fluxos.Add(fluxo);

                    await _context.SaveChangesAsync();
                    lote = await _context.Lote
                            .Include(l => l.Fluxos)
                            .FirstOrDefaultAsync(l => l.Id == idLote);
                    response.Dados = lote.Fluxos.LastOrDefault().Id;
                }              
            }
            catch
            {
                response.Status = false;
                response.Mensagem = "Falha ao inserir fluxo";                
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> SalvarFluxo(int idUsuario, int idProjeto, string numeroLote, int idSituacao, string observacao)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var lote = await _context.Lote
                .Include(l => l.Fluxos)
                .FirstOrDefaultAsync(l => l.NumLote == numeroLote && l.ProjetoId == idProjeto);

                
                if (lote == null)
                    throw new Exception("Lote não existe!.");

                var valida = await ValidaFluxo(lote.Id, idSituacao);

                if (!valida.Item1)
                {
                    response.Status = false;
                    response.Mensagem = $"O lote {numeroLote} não está na fila selecionada!";
                    return response;
                }


                var fluxo = new FluxoModel()
                {
                    SituacaoId = idSituacao,
                    UsuarioId = idUsuario,
                    DtFim = DateTime.Now,
                    LoteId = lote.Id,
                    Observacao = observacao
                };

                lote.Fluxos.Add(fluxo);

                await _context.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Mensagem = "Falha ao salvar fluxo | "+ ex.Message;
            }

            return response;
        }

        private async Task<Tuple<bool, int>> ValidaFluxo(int idLote, int idSituacao)
        {
            bool valido = false;
            int ultimoId = 0;

            
            
            var v = await _context.FluxoLote
                     .Where(f => f.LoteId == idLote)
                     .OrderByDescending(f => f.Id)
                     .FirstOrDefaultAsync();

            if (v == null) return new Tuple<bool, int>(valido, ultimoId);

            if ( v.DtFim != null)
            {
                var FilaAnterior = await _context.Situacoes.FindAsync(idSituacao);
                if (FilaAnterior.IdSituacaoAnterior == v.SituacaoId)
                {
                    valido = true;
                }
            }
            else if (v.SituacaoId == idSituacao)
            {
                valido = true;    
                ultimoId = v.Id;
            }

            Tuple<bool, int> response = new Tuple<bool, int>(valido, ultimoId);
            return response;
        }

    }
}
