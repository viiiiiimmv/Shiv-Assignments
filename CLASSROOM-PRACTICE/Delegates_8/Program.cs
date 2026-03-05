namespace Delegates_8;

using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
AI TASK PROCESSING ENGINE
==========================================================

STORY
An AI engine processes various types of tasks such as
data processing, image analysis, and security scans.

Each task has a priority level and execution logic.

The system must support dynamic filtering, ordering,
and analytics over tasks.

Your task is to implement the system using:

- OOP
- Delegates
- Functional Programming
- LINQ analytics

----------------------------------------------------------
TASKS TO IMPLEMENT
----------------------------------------------------------

1️⃣ Create abstract class AITask

Properties
- int TaskId
- string Name
- int Priority

Method
- abstract void Execute()

RULES
Each task type should implement its own execution logic.
Execution may simply print a message indicating the task
is running.

*/
public abstract class AITask
{
    public int TaskId { get; set; }
    public string Name { get; set; }
    public int Priority { get; set; }

    public abstract void Execute();
}

public class DataProcessingTask : AITask
{
    public override void Execute()
    {
        Console.WriteLine($"Running data processing task : {Name}");
    }
}

public class ImageProcessingTask : AITask
{
    public override void Execute()
    {
        Console.WriteLine($"Running image processing task : {Name}");
    }
}

public class SecurityScanTask : AITask
{
    public override void Execute()
    {
        Console.WriteLine($"Running security scan task : {Name}");
    }
}

/*
----------------------------------------------------------

2️⃣ Create derived classes

DataProcessingTask
ImageProcessingTask
SecurityScanTask

Override Execute()

Example behavior

DataProcessingTask
Print "Running data processing task: {Name}"

ImageProcessingTask
Print "Running image processing task: {Name}"

SecurityScanTask
Print "Running security scan task: {Name}"

*/
public delegate bool TaskFilter(AITask task);
/*
----------------------------------------------------------

3️⃣ Create delegate

public delegate bool TaskFilter(AITask task);

This delegate allows dynamic task filtering.


----------------------------------------------------------

4️⃣ Create class TaskManager

Fields
- private List<AITask> tasks

Methods to implement

AddTask(AITask task)

GetTasks()

ProcessTasks(TaskFilter filter)

RULES
- Use delegate to determine which tasks should execute
- Execute tasks that match the filter
- Call Execute() on each eligible task


OrderTasks(Func<AITask,int> keySelector)

RULES
- Sort tasks by provided key
- Use OrderByDescending
- Return ordered list

*/
public class TaskManager
{
    private List<AITask> _tasks = new List<AITask>();

    public void AddTask(AITask task)
    {
        _tasks.Add((task));
    }

    public List<AITask> GetTasks()
    {
        return _tasks;
    }

    public void ProcessTasks(TaskFilter filter)
    {
        foreach (var task in _tasks.Where(task => filter(task)))
        {
            task.Execute();
        }
    }

    public List<AITask> OrderTasks(Func<AITask, int> keySelector)
    {
        return _tasks
            .OrderByDescending(keySelector)
            .ToList();
    }

    public List<AITask> HighPriorityTasks()
    {
        return _tasks.Where(t => t.Priority > 7).ToList();
    }

    public List<List<AITask>> GroupByTypes()
    {
        return _tasks
            .GroupBy(x => x.GetType())
            .Select(x => x.ToList())
            .ToList();
    }
    
    public List<(string TaskName, double AveragePriority)> AveragePriority()
    {
        return _tasks
            .GroupBy(x => x.GetType().Name)
            .Select(t => (t.Key, t.Average(p => p.Priority)))
            .ToList();
    }

    public List<AITask> Top3Tasks()
    {
        return _tasks.OrderByDescending(x => x.Priority).Take(3).ToList();
    }
}
/*
----------------------------------------------------------

5️⃣ LINQ ANALYTICS REQUIREMENTS

Implement the following reports

A) High priority tasks

Rules
Priority > 7


B) Group tasks by type

Example groups
DataProcessingTask
ImageProcessingTask
SecurityScanTask


C) Average priority per type

Rules
GroupBy task type
Calculate average priority


D) Top 3 tasks

Rules
Sort by priority
Take top 3


----------------------------------------------------------

6️⃣ DO NOT MODIFY MAIN()

Main assumes the required classes and
methods are correctly implemented.

==========================================================
*/

class Program
{
    static void Main()
    {
        TaskManager manager = new TaskManager();

        //--------------------------------------------------
        // CREATE TASKS
        //--------------------------------------------------

        AITask t1 = new DataProcessingTask
        {
            TaskId = 1,
            Name = "User Analytics",
            Priority = 9
        };

        AITask t2 = new ImageProcessingTask
        {
            TaskId = 2,
            Name = "Satellite Image Analysis",
            Priority = 8
        };

        AITask t3 = new SecurityScanTask
        {
            TaskId = 3,
            Name = "Network Vulnerability Scan",
            Priority = 10
        };

        AITask t4 = new DataProcessingTask
        {
            TaskId = 4,
            Name = "Log Processing",
            Priority = 6
        };

        AITask t5 = new ImageProcessingTask
        {
            TaskId = 5,
            Name = "Medical Image Detection",
            Priority = 7
        };

        AITask t6 = new SecurityScanTask
        {
            TaskId = 6,
            Name = "Malware Detection",
            Priority = 9
        };

        manager.AddTask(t1);
        manager.AddTask(t2);
        manager.AddTask(t3);
        manager.AddTask(t4);
        manager.AddTask(t5);
        manager.AddTask(t6);

        //--------------------------------------------------
        // DEFINE FILTER USING DELEGATE
        //--------------------------------------------------

        TaskFilter highPriorityFilter =
            task => task.Priority >= 8;

        Console.WriteLine("\nPROCESSING HIGH PRIORITY TASKS\n");

        manager.ProcessTasks(highPriorityFilter);

        //--------------------------------------------------
        // ORDER TASKS BY PRIORITY
        //--------------------------------------------------

        Console.WriteLine("\nTASKS ORDERED BY PRIORITY\n");

        var orderedTasks = manager.OrderTasks(t => t.Priority);

        foreach (var task in orderedTasks)
        {
            Console.WriteLine($"{task.Name} - Priority: {task.Priority}");
        }

        //--------------------------------------------------
        // HIGH PRIORITY TASKS
        //--------------------------------------------------

        Console.WriteLine("\nHIGH PRIORITY TASKS (Priority > 7)\n");

        var highPriority = manager
            .GetTasks()
            .Where(t => t.Priority > 7);

        foreach (var task in highPriority)
        {
            Console.WriteLine($"{task.Name} - {task.Priority}");
        }

        //--------------------------------------------------
        // GROUP TASKS BY TYPE
        //--------------------------------------------------

        Console.WriteLine("\nGROUP TASKS BY TYPE\n");

        var grouped = manager
            .GetTasks()
            .GroupBy(t => t.GetType().Name);

        foreach (var group in grouped)
        {
            Console.WriteLine($"Type: {group.Key}");

            foreach (var task in group)
            {
                Console.WriteLine($"   {task.Name}");
            }
        }

        //--------------------------------------------------
        // AVERAGE PRIORITY PER TYPE
        //--------------------------------------------------

        Console.WriteLine("\nAVERAGE PRIORITY PER TYPE\n");

        var avgPriority = manager
            .GetTasks()
            .GroupBy(t => t.GetType().Name)
            .Select(g => new
            {
                Type = g.Key,
                AvgPriority = g.Average(x => x.Priority)
            });

        foreach (var item in avgPriority)
        {
            Console.WriteLine($"{item.Type} -> {item.AvgPriority}");
        }

        //--------------------------------------------------
        // TOP 3 TASKS
        //--------------------------------------------------

        Console.WriteLine("\nTOP 3 TASKS BY PRIORITY\n");

        var topTasks = manager
            .GetTasks()
            .OrderByDescending(t => t.Priority)
            .Take(3);

        foreach (var task in topTasks)
        {
            Console.WriteLine($"{task.Name} - {task.Priority}");
        }
    }
}