namespace SGD.Models
{
    public class MetadadosTipoDocModel
    {
        public int Id { get; set; }
        public int MetadadoId { get; set; }
        public MetadadosModel Metadado { get; set; }

        public int TipoDocumentalId { get; set; }
        public TipoDocumentalModel TipoDocumental { get; set; }
    }
}
