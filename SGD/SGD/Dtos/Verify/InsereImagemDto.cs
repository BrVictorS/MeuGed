namespace SGD.Dtos.Verify
{
    public class InsereImagemDto
    {
        public IFormFileCollection files {  get; set; }
        public string idLote {  get; set; }
        public int posicao { get; set; }
    }
}
