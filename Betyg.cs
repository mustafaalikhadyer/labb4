using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1
{
    [Table("Betyg")]
    public class Betyg
    {
        [Key]
        public int BetygID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public int KursID { get; set; }

        [Required]
        public int LarareID { get; set; }

        [Required]
        public string BetygVarde { get; set; } = string.Empty; 
        [Required]
        public DateTime Datum { get; set; }

        public Student Student { get; set; } = null!;
        public Kurs Kurs { get; set; } = null!;
        public Personal Larare { get; set; } = null!;

    }
}
