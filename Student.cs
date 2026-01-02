using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1
{
    [Table("Student")]
    public class Student
    {
        public int StudentID { get; set; }
        public string Fornamn { get; set; } = null!;
        public string Efternamn { get; set; } = null!;
        public string Personnummer { get; set; } = null!;

        public int KlassID { get; set; }
        public Klass Klass { get; set; } = null!;
    }
}
