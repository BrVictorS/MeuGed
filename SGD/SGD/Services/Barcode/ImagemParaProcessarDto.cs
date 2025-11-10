
using System.Text;

namespace SGD.Services.Barcode
{
    internal class ImagemParaProcessarDto : TextWriter
    {
        public string Id { get; set; }
        public string Caminho { get; set; }

        public override Encoding Encoding => throw new NotImplementedException();
    }
}