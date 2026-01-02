using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1
{
    [Table("Klass")]
    public class Klass
    {
        [Key]
        public int KlassID { get; set; }

        [Required]
        public string KlassNamn { get; set; } = string.Empty; 

        // Navigation properties
        public ICollection<Student> Studenter { get; set; } = new List<Student>();
        public int? AnsvarigLarareID { get; set; }
        public Personal? AnsvarigLarare { get; set; }

    }
}
