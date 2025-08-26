using System.ComponentModel.DataAnnotations;

namespace SGD.Models
{
    public class ParametrosModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }
        [Required]
        public string Valor {  get; set; }

    }
}
