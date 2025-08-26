namespace SGD.Models
{
    public class ProjetoPermissaoModel
    {
        public int ProjetoId { get; set; }
        public ProjetoModel Projeto { get; set; }

        public int PermissaoId { get; set; }
        public PermissoesModel Permissao { get; set; }
    }
}
