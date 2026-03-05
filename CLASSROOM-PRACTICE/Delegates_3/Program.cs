namespace Delegates_3;
using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
MAGICAL ACADEMY CERTIFICATION SYSTEM
==========================================================

STORY
The Arcane Academy trains students in magical disciplines.
Each course evaluates students using different eligibility
rules before granting a magical certificate.

When a student passes a course, a certificate event
is triggered.

Your task is to design the system using:

- OOP
- Delegates
- Events
- LINQ

----------------------------------------------------------
TASKS TO IMPLEMENT
----------------------------------------------------------

1️⃣ Create class Student

Properties
- int Id
- string Name
- int MagicScore
- int Attendance
*/
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MagicScore { get; set; }
    public int Attendance { get; set; }
}
/*

----------------------------------------------------------

2️⃣ Create abstract class Course

Properties
- int CourseId
- string Title
- int Duration

Delegate inside class
public delegate bool EligibilityRule(Student s);

Event
public event Action<Student> OnCertificateGranted;

Methods to implement

void EvaluateStudent(Student s, EligibilityRule rule)

RULES
- Use the delegate to determine if student is eligible
- If eligible:
    - Print message: "Certificate granted to {student name}"
    - Trigger event OnCertificateGranted
- If not eligible:
    - Print message: "Student {student name} failed eligibility"

*/
public abstract class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }

    public delegate bool EligibilityRule(Student s);

    public event Action<Student> OnCertificateGranted;

    public void EvaluateStudent(Student s, EligibilityRule rule)
    {
        if (rule(s))
        {
            Console.WriteLine($"Certificate granted to {s.Name}");
            OnCertificateGranted.Invoke(s);
        }
        else
        {
            Console.WriteLine($"{s.Name} failed certification");
        }
    }
}
/*
----------------------------------------------------------

3️⃣ Create derived classes

SpellCourse
PotionCourse

RULES

SpellCourse eligibility example
- MagicScore >= 75
- Attendance >= 70

PotionCourse eligibility example
- MagicScore >= 65
- Attendance >= 60

You may override behavior if needed.

*/
class SpellCourse : Course
{
    public static bool SpellEligibility(Student s)
    {
        return s.MagicScore >= 75 && s.Attendance >= 70;
    }
}

class PotionCourse : Course
{
    public static bool PotionEligibility(Student s)
    {
        return s.MagicScore >= 65 && s.Attendance >= 60;
    }
}
/*
----------------------------------------------------------

4️⃣ LINQ OPERATIONS

Create static helper methods OR implement them inside Main.

Required queries:

A) Students with MagicScore > 80

B) Group students by Attendance range

Ranges:
0–50
51–75
76–100

C) Top 5 performers by MagicScore


----------------------------------------------------------

5️⃣ EVENT HANDLING

Subscribe to OnCertificateGranted event.

Example event action:
Print

"Academy Registry: Certificate recorded for {student}"



----------------------------------------------------------

6️⃣ DO NOT MODIFY MAIN()

The main method assumes all the above classes and
methods are correctly implemented.

==========================================================
*/

class Program
{
    static void Main()
    {
        //--------------------------------------------------
        // CREATE STUDENTS
        //--------------------------------------------------

        List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "Aether", MagicScore = 92, Attendance = 85 },
            new Student { Id = 2, Name = "Lyra", MagicScore = 81, Attendance = 75 },
            new Student { Id = 3, Name = "Drako", MagicScore = 60, Attendance = 65 },
            new Student { Id = 4, Name = "Nova", MagicScore = 88, Attendance = 90 },
            new Student { Id = 5, Name = "Orion", MagicScore = 72, Attendance = 55 },
            new Student { Id = 6, Name = "Mira", MagicScore = 95, Attendance = 98 },
            new Student { Id = 7, Name = "Zane", MagicScore = 45, Attendance = 40 }
        };

        //--------------------------------------------------
        // CREATE COURSES
        //--------------------------------------------------

        Course spellCourse = new SpellCourse
        {
            CourseId = 101,
            Title = "Advanced Spell Casting",
            Duration = 6
        };

        Course potionCourse = new PotionCourse
        {
            CourseId = 102,
            Title = "Potion Mastery",
            Duration = 4
        };

        //--------------------------------------------------
        // EVENT SUBSCRIPTION
        //--------------------------------------------------

        spellCourse.OnCertificateGranted += s =>
        {
            Console.WriteLine($"Academy Registry: Certificate recorded for {s.Name}");
        };

        potionCourse.OnCertificateGranted += s =>
        {
            Console.WriteLine($"Academy Registry: Certificate recorded for {s.Name}");
        };

        //--------------------------------------------------
        // DEFINE ELIGIBILITY RULES
        //--------------------------------------------------

        Course.EligibilityRule spellRule =
            s => s.MagicScore >= 75 && s.Attendance >= 70;

        Course.EligibilityRule potionRule =
            s => s.MagicScore >= 65 && s.Attendance >= 60;

        //--------------------------------------------------
        // EVALUATE STUDENTS
        //--------------------------------------------------

        Console.WriteLine("\nSPELL COURSE EVALUATION\n");

        foreach (var s in students)
        {
            spellCourse.EvaluateStudent(s, spellRule);
        }

        Console.WriteLine("\nPOTION COURSE EVALUATION\n");

        foreach (var s in students)
        {
            potionCourse.EvaluateStudent(s, potionRule);
        }

        //--------------------------------------------------
        // LINQ REPORTS
        //--------------------------------------------------

        Console.WriteLine("\nSTUDENTS WITH SCORE > 80");

        var highScore = students
            .Where(s => s.MagicScore > 80);

        foreach (var s in highScore)
        {
            Console.WriteLine($"{s.Name} - {s.MagicScore}");
        }

        //--------------------------------------------------

        Console.WriteLine("\nGROUP BY ATTENDANCE RANGE");

        var grouped = students.GroupBy(s =>
        {
            if (s.Attendance <= 50) return "0-50";
            if (s.Attendance <= 75) return "51-75";
            return "76-100";
        });

        foreach (var g in grouped)
        {
            Console.WriteLine($"Range {g.Key}");

            foreach (var s in g)
            {
                Console.WriteLine($"   {s.Name} - {s.Attendance}");
            }
        }

        //--------------------------------------------------

        Console.WriteLine("\nTOP 5 PERFORMERS");

        var top = students
            .OrderByDescending(s => s.MagicScore)
            .Take(5);

        foreach (var s in top)
        {
            Console.WriteLine($"{s.Name} - {s.MagicScore}");
        }
    }
}