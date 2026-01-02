using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1
{
    [Table("Kurs")]
    public class Kurs
    {
        [Key]
        public int KursID { get; set; }

        [Required]
        public string KursNamn { get; set; } = string.Empty; // Fixar warning CS8618
    }
}
