using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            using var context = new SkolaContext();

            while (true)
            {
                Console.WriteLine("\n--- Skoladministration ---");
                Console.WriteLine("1. Visa alla studenter");
                Console.WriteLine("2. Visa studenter i klass");
                Console.WriteLine("3. Lägg till student");
                Console.WriteLine("4. Visa personal");
                Console.WriteLine("5. Lägg till personal");
                Console.WriteLine("6. Visa antal lärare per avdelning");
                Console.WriteLine("7. Visa studentinformation med kurser och betyg");
                Console.WriteLine("8. Visa alla kurser");
                Console.WriteLine("9. Sätt betyg på student");
                Console.WriteLine("0. Avsluta");

                string? val = Console.ReadLine();

                switch (val)
                {
                    case "1":
                        VisaAllaStudenter(context);
                        break;
                    case "2":
                        VisaStudenterIKlass(context);
                        break;
                    case "3":
                        LaggaTillStudent(context);
                        break;
                    case "4":
                        VisaPersonal(context);
                        break;
                    case "5":
                        LaggaTillPersonal(context);
                        break;
                    case "6":
                        VisaLararePerAvdelning(context);
                        break;
                    case "7":
                        VisaStudentInfo(context);
                        break;
                    case "8":
                        VisaKurser(context);
                        break;
                    case "9":
                        SattBetyg(context);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val");
                        break;
                }
            }
        }

        // --- Studentmetoder ---
        static void VisaAllaStudenter(SkolaContext context)
        {
            var studenter = context.Studenter.Include(s => s.Klass).ToList();
            foreach (var s in studenter)
            {
                Console.WriteLine($"{s.Fornamn} {s.Efternamn} ({s.Personnummer}) - Klass: {s.Klass.KlassNamn}");
            }
        }

        static void VisaStudenterIKlass(SkolaContext context)
        {
            var klasser = context.Klasser.ToList();
            for (int i = 0; i < klasser.Count; i++)
                Console.WriteLine($"{i + 1}. {klasser[i].KlassNamn}");

            Console.Write("Välj klassnummer: ");
            if (!int.TryParse(Console.ReadLine(), out int val) || val < 1 || val > klasser.Count)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }

            var klass = klasser[val - 1];
            var studenter = context.Studenter.Where(s => s.KlassID == klass.KlassID).ToList();

            foreach (var s in studenter)
                Console.WriteLine($"{s.Fornamn} {s.Efternamn}");
        }

        static void LaggaTillStudent(SkolaContext context)
        {
            Console.Write("Förnamn: ");
            string fornamn = Console.ReadLine()!;
            Console.Write("Efternamn: ");
            string efternamn = Console.ReadLine()!;
            Console.Write("Personnummer: ");
            string personnummer = Console.ReadLine()!;

            var klasser = context.Klasser.AsNoTracking().ToList();
            for (int i = 0; i < klasser.Count; i++)
                Console.WriteLine($"{i + 1}. {klasser[i].KlassNamn}");

            Console.Write("Välj klassnummer för studenten: ");
            if (!int.TryParse(Console.ReadLine(), out int val) || val < 1 || val > klasser.Count)
            {
                Console.WriteLine("Ogiltigt val, studenten läggs inte till.");
                return;
            }

            var student = new Student
            {
                Fornamn = fornamn,
                Efternamn = efternamn,
                Personnummer = personnummer,
                KlassID = klasser[val - 1].KlassID
            };

            context.Studenter.Add(student);
            context.SaveChanges();
            Console.WriteLine("Student tillagd!");
        }

        // --- Personalmetoder ---
        static void VisaPersonal(SkolaContext context)
        {
            var personal = context.Personal.ToList();
            foreach (var p in personal)
                Console.WriteLine($"{p.Fornamn} {p.Efternamn} ({p.Befattning})");
        }

        static void LaggaTillPersonal(SkolaContext context)
        {
            Console.Write("Förnamn: ");
            string fornamn = Console.ReadLine()!;
            Console.Write("Efternamn: ");
            string efternamn = Console.ReadLine()!;
            Console.Write("Personnummer: ");
            string personnummer = Console.ReadLine()!;
            Console.Write("Befattning: ");
            string befattning = Console.ReadLine()!;

            var personal = new Personal
            {
                Fornamn = fornamn,
                Efternamn = efternamn,
                Personnummer = personnummer,
                Befattning = befattning
            };

            context.Personal.Add(personal);
            context.SaveChanges();
            Console.WriteLine("Personal tillagd!");
        }

        // --- Extra funktioner ---
        static void VisaLararePerAvdelning(SkolaContext context)
        {
            var result = context.Personal
                .Where(p => p.Befattning == "Lärare")
                .GroupBy(p => p.Befattning)
                .Select(g => new { Avdelning = g.Key, Antal = g.Count() })
                .ToList();

            foreach (var r in result)
                Console.WriteLine($"{r.Antal} lärare på avdelning: {r.Avdelning}");
        }

        static void VisaStudentInfo(SkolaContext context)
        {
            var studenter = context.Studenter.Include(s => s.Klass).ToList();

            foreach (var s in studenter)
            {
                Console.WriteLine($"{s.Fornamn} {s.Efternamn} - Klass: {s.Klass.KlassNamn}");

                var betyg = context.Betyg
                    .Where(b => b.StudentID == s.StudentID)
                    .Include(b => b.Kurs)
                    .Include(b => b.Larare)
                    .ToList();

                foreach (var b in betyg)
                    Console.WriteLine($"   Kurs: {b.Kurs.KursNamn}, Betyg: {b.BetygVarde}, Lärare: {b.Larare.Fornamn} {b.Larare.Efternamn}, Datum: {b.Datum:d}");
            }
        }

        static void VisaKurser(SkolaContext context)
        {
            var kurser = context.Kurser.ToList();
            foreach (var k in kurser)
                Console.WriteLine(k.KursNamn);
        }

        static void SattBetyg(SkolaContext context)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                Console.Write("StudentID: ");
                int studentId = int.Parse(Console.ReadLine()!);

                Console.Write("KursID: ");
                int kursId = int.Parse(Console.ReadLine()!);

                Console.Write("LärareID: ");
                int larareId = int.Parse(Console.ReadLine()!);

                Console.Write("Betyg (A-F): ");
                string betyg = Console.ReadLine()!;

                var nyttBetyg = new Betyg
                {
                    StudentID = studentId,
                    KursID = kursId,
                    LarareID = larareId,
                    BetygVarde = betyg,
                    Datum = DateTime.Now
                };

                context.Betyg.Add(nyttBetyg);
                context.SaveChanges();
                transaction.Commit();
                Console.WriteLine("Betyg satt!");
            }
            catch
            {
                transaction.Rollback();
                Console.WriteLine("Fel uppstod – betyg sattes inte!");
            }
        }
    }
}
