using System.ComponentModel.DataAnnotations;

namespace GedDbApi.Dto
{
    public class SituacaoImagemUpdateDto
    {
        [Required(ErrorMessage ="Imagem nao informada")]
        public string Imagem { get; set; }
        [Required(ErrorMessage ="situacao nao informada")]
        public string Situacao { get; set; }
    }
}
