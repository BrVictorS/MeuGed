namespace SGD.Models
{
    public class FuncaoModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Funcao { get; set; }
        public List<SubfuncaoModel> Subfuncoes { get; set; }       
    }
}
