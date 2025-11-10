namespace SGD.Dtos.Verify
{
    public class InsereDocumentoDto
    {
        public string id {  get; set; } //id da imagem
        public string documento { get; set; }
        public string LoteId { get; set; }
        public string remover { get; set; } = "0";
    }
}
