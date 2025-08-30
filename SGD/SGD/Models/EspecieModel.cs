namespace SGD.Models
{
    public class EspecieModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Especie { get; set; }
        public ICollection<EspecieTipoDoc> especieTiposDoc { get; set; }
    }
}