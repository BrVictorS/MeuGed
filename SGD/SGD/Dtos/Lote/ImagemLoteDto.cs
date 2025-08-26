using System;

namespace SGD.Dtos.Lote
{
    public class ImagemLoteDto
    {
        public string id { get; set; }
        public string caminho { get; set; }
        public string situacao { get; set; } // string para facilitar o post; você converte para enum na controller
        public string? documentoId { get; set; }
    }
}
