namespace Delegates_6;

using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
HOSPITAL TRIAGE PRIORITY SYSTEM
==========================================================

STORY
A hospital triage system evaluates incoming patients and
assigns treatment priority dynamically.

Different hospitals may use different priority logic
based on:

- Severity
- Age
- Department
- Emergency conditions

The system must support flexible priority rules.

Your task is to implement this using:

- Delegates
- Functional Programming
- LINQ
- Dynamic sorting strategies

----------------------------------------------------------
TASKS TO IMPLEMENT
----------------------------------------------------------
*/
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int SeverityLevel { get; set; }
    public string Department { get; set; }
}
/*
1️⃣ Create class Patient

Properties
- int Id
- string Name
- int Age
- int SeverityLevel
- string Department

Example Departments
Emergency
Cardiology
Neurology
Orthopedics


----------------------------------------------------------

2️⃣ Create delegate

public delegate int PriorityCalculator(Patient p);

This delegate determines the patient priority score.

Higher score = Higher priority.

*/
public delegate int PriorityCalculator(Patient p);
/*
----------------------------------------------------------

3️⃣ Create class TriageSystem

Field
- private List<Patient> patients

Methods to implement

AddPatient(Patient p)

GetPatients()

AssignPriority(Patient p, PriorityCalculator calc)

RULES
- Use the delegate to compute priority score
- Print result

Example output
"Patient John assigned priority 85"


SortPatients(Func<Patient,int> keySelector)

RULES
- Sort patients by provided key
- Use OrderByDescending
- Return sorted list

*/
public class TriageSystem
{
    private List<Patient> _patients = new List<Patient>();

    public void AddPatient(Patient patient)
    {
        _patients.Add(patient);
    }

    public List<Patient> GetPatients()
    {
        return _patients;
    }

    public void AssignPriority(Patient patient, PriorityCalculator calc)
    {
        int priority = calc(patient);
        Console.WriteLine($"Patient {patient.Name} has been assigned priority {priority}");
    }

    public List<Patient> SortPatients(Func<Patient, int> keySelector)
    {
        return _patients.OrderByDescending(keySelector).ToList();
    }

    public List<Patient> CriticalPatients()
    {
        return _patients.Where(p => p.SeverityLevel > 8).ToList();
    }

    public List<(string Department, double AvgAge)> AverageAgePerDepartment()
    {
        return _patients
            .GroupBy(p => p.Department)
            .Select(g => (g.Key, g.Average(x => x.Age)))
            .ToList();
    }

    public List<Patient> Top5Priority(PriorityCalculator calc)
    {
        return _patients
            .OrderByDescending(p => calc(p))
            .Take(5)
            .ToList();
    }

    public List<Patient> Top5Priority()
    {
        return _patients.OrderByDescending(p => p.SeverityLevel).Take(5).ToList();
    }
}
/*
----------------------------------------------------------

4️⃣ LINQ REPORT REQUIREMENTS

Implement these reports

A) Critical patients

Rules
SeverityLevel > 8


B) Average age per department

Rules
GroupBy Department
Average Age


C) Highest severity per department

Rules
GroupBy Department
Select patient with max SeverityLevel


D) Top 5 priority patients

Rules
Sort by priority score
Take top 5


----------------------------------------------------------

5️⃣ DO NOT MODIFY MAIN()

Main assumes the required classes and methods
are correctly implemented.

==========================================================
*/

class Program
{
    static void Main()
    {
        TriageSystem triage = new TriageSystem();

        //--------------------------------------------------
        // CREATE PATIENTS
        //--------------------------------------------------

        List<Patient> patients = new List<Patient>
        {
            new Patient { Id = 1, Name = "John", Age = 45, SeverityLevel = 9, Department = "Emergency" },
            new Patient { Id = 2, Name = "Mira", Age = 60, SeverityLevel = 7, Department = "Cardiology" },
            new Patient { Id = 3, Name = "Ayaan", Age = 30, SeverityLevel = 10, Department = "Emergency" },
            new Patient { Id = 4, Name = "Lena", Age = 25, SeverityLevel = 6, Department = "Orthopedics" },
            new Patient { Id = 5, Name = "Victor", Age = 70, SeverityLevel = 8, Department = "Neurology" },
            new Patient { Id = 6, Name = "Sara", Age = 50, SeverityLevel = 9, Department = "Cardiology" },
            new Patient { Id = 7, Name = "Noah", Age = 33, SeverityLevel = 5, Department = "Orthopedics" },
            new Patient { Id = 8, Name = "Eva", Age = 41, SeverityLevel = 10, Department = "Emergency" }
        };

        foreach (var p in patients)
        {
            triage.AddPatient(p);
        }

        //--------------------------------------------------
        // DEFINE PRIORITY LOGIC USING DELEGATE
        //--------------------------------------------------

        PriorityCalculator emergencyPriority =
            p => p.SeverityLevel * 10 + p.Age / 5;

        PriorityCalculator cardioPriority =
            p => p.SeverityLevel * 8 + p.Age / 3;

        Console.WriteLine("\nPATIENT PRIORITIES\n");

        foreach (var p in triage.GetPatients())
        {
            triage.AssignPriority(p, emergencyPriority);
        }

        //--------------------------------------------------
        // SORT PATIENTS BY SEVERITY
        //--------------------------------------------------

        Console.WriteLine("\nPATIENTS SORTED BY SEVERITY\n");

        var sortedBySeverity = triage.SortPatients(p => p.SeverityLevel);

        foreach (var p in sortedBySeverity)
        {
            Console.WriteLine($"{p.Name} - Severity: {p.SeverityLevel}");
        }

        //--------------------------------------------------
        // CRITICAL PATIENTS
        //--------------------------------------------------

        Console.WriteLine("\nCRITICAL PATIENTS (Severity > 8)\n");

        var critical = triage
            .GetPatients()
            .Where(p => p.SeverityLevel > 8);

        foreach (var p in critical)
        {
            Console.WriteLine($"{p.Name} - Severity: {p.SeverityLevel}");
        }

        //--------------------------------------------------
        // AVERAGE AGE PER DEPARTMENT
        //--------------------------------------------------

        Console.WriteLine("\nAVERAGE AGE PER DEPARTMENT\n");

        var avgAge = triage
            .GetPatients()
            .GroupBy(p => p.Department)
            .Select(g => new
            {
                Department = g.Key,
                AvgAge = g.Average(x => x.Age)
            });

        foreach (var g in avgAge)
        {
            Console.WriteLine($"{g.Department} -> {g.AvgAge}");
        }

        //--------------------------------------------------
        // HIGHEST SEVERITY PER DEPARTMENT
        //--------------------------------------------------

        Console.WriteLine("\nHIGHEST SEVERITY PER DEPARTMENT\n");

        var highest = triage
            .GetPatients()
            .GroupBy(p => p.Department)
            .Select(g => g.OrderByDescending(x => x.SeverityLevel).First());

        foreach (var p in highest)
        {
            Console.WriteLine($"{p.Department} -> {p.Name} ({p.SeverityLevel})");
        }

        //--------------------------------------------------
        // TOP 5 PRIORITY PATIENTS
        //--------------------------------------------------

        Console.WriteLine("\nTOP 5 PRIORITY PATIENTS\n");

        var top = triage
            .GetPatients()
            .OrderByDescending(p => emergencyPriority(p))
            .Take(5);

        foreach (var p in top)
        {
            Console.WriteLine($"{p.Name} - Priority: {emergencyPriority(p)}");
        }
    }
}