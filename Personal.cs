using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1
{
    [Table("Personal")]
    public class Personal
    {
        [Key]
        public int PersonalID { get; set; }

        [Required]
        public string Fornamn { get; set; } = string.Empty; 

        [Required]
        public string Efternamn { get; set; } = string.Empty; 

        [Required]
        public string Personnummer { get; set; } = string.Empty; 

        [Required]
        public string Befattning { get; set; } = string.Empty; 

        
        public DateTime AnstalldDatum { get; set; }
        public decimal Manadslon { get; set; }
    }
}