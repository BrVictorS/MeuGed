namespace SGD.Models
{
    public class AtividadeModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Atividade { get; set; }
        public ICollection<EspecieModel> Especies { get; set; }
    }
}