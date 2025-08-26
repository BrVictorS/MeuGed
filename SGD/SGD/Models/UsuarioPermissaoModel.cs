namespace SGD.Models
{
    public class UsuarioPermissaoModel
    {
        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }

        public int PermissaoId { get; set; }
        public PermissoesModel Permissao { get; set; }
    }
}
