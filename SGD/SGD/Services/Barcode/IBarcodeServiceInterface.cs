using SGD.Dtos.Lote;
using SGD.Dtos.Response;

namespace SGD.Services.Barcode
{
    public interface IBarcodeServiceInterface
    {
        public void ConfigurarCodigoBarras();

        public string CapturarCodigoBarras(string v_sImagem);
        public void QuebraLote(LoteApiDto lote);
    }
}
