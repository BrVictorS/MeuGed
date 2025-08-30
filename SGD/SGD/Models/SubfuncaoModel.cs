using System.Text.Json.Serialization;

namespace SGD.Models
{
    public class SubfuncaoModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }

        public string Subfuncao { get; set; }
        public int FuncaoId { get; set; }
        [JsonIgnore]
        public FuncaoModel Funcao { get; set; }

        public ICollection<AtividadeModel> Atividades { get; set; }
    }
}