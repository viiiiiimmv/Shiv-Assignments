namespace QUESTION_3;

using System;
using System.Collections.Generic;

/* ================= INTERFACE ================= */

public interface IStudent
{
    int Id { get; }
    string Name { get; set; }

    void AddMarks(string subject, int marks);
    double GetAverageMarks();
}

/* =================================================
   IMPLEMENT BELOW CLASS

   class Student : IStudent

   Requirements:
   - Store subject-wise marks using Dictionary<string, int>
   - Average should be computed dynamically
   ================================================= */
public class Student : IStudent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string , int> Marks { get; set; }

    public Student(int id, string name)
    {
        Id = id;
        Name = name;
        Marks = new Dictionary<string , int>();
    }

    public void AddMarks(string subject, int marks)
    {
        Marks.Add(subject, marks);
    }

    public double GetAverageMarks()
    {
        double sum = Marks.Values.Sum();
        return sum / Marks.Count;
    }
}

class Program
{
    static void Main()
    {
        IStudent s1 = new Student(1, "Aman");

        s1.AddMarks("Math", 90);
        s1.AddMarks("Science", 80);
        s1.AddMarks("English", 85);

        Console.WriteLine("Average Marks: " + s1.GetAverageMarks());
    }
}