using System;

namespace StudentGrades
{
    // Simple class with properties
    public class Student
    {
        public int Id { get; }
        public string Name { get; }
        public int Marks { get; }
        public string Grade { get; private set; } = "";

        public Student(int id, string name, int marks)
        {
            Id = id;
            Name = name;
            Marks = marks;
        }

        public override string ToString() => $"Student[Id={Id}, {Name}, Marks={Marks}]";
    }

    class Program
    {
        // PATTERN MATCHING METHOD - Main feature demo
        static string GetGrade(Student student) => student.Marks switch
        {
            >= 90 => "A - Excellent!",
            >= 80 => "B - Very Good!",
            >= 70 => "C - Good",
            >= 60 => "D - Pass",
            >= 50 => "E - Average",
            _ => "F - Failed - Retake"
        };

        static void Main()
        {
            // Create students
            Student[] students = {
                new Student(1, "Alice", 92),
                new Student(2, "Bob", 78),
                new Student(3, "Charlie", 55),
                new Student(4, "Diana", 45)
            };

            // Process each student using pattern matching
            Console.WriteLine("Name\tGrade");
            foreach (Student s in students)
            {
                string grade = GetGrade(s);
                Console.WriteLine($"{s.Name}\t{grade}");
            }
        }
    }
}