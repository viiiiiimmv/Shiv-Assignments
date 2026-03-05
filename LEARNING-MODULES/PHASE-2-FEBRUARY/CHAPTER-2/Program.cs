using System;
using System.Text;
using System.Xml.Serialization;

namespace CHAPTER_2;


class Alarm
{
    public delegate void AlarmHandler();

    public event AlarmHandler? AlarmRang;

    public void Ring()
    {
        Console.WriteLine("Alarm ringing...");
        AlarmRang?.Invoke();
    }
}

// * Delegates
class MathematicalOperators
{
    delegate int MathOperation(int x, int y);
    
    static int Add(int x, int y)
    {
        return x + y;
    }

    static int Subtract(int x, int y)
    {
        return x - y;
    }
    
    // ? MathOperation mp = Add;
    // * Console.WriteLine(mp(90,12));
    
    
    // ! Multicast Delegates
    delegate void Notify();

    static void Email()
    {
        Console.WriteLine("Email notification");
    }
    
    static void Phone()
    {
        Console.WriteLine("Phone notification");
    }
    
    // * Notify notify = Email
    // * notify += Phone
    // * notify();
}

[Serializable]
public class User
{
    public string Name { get; set; }
    public int Id { get; set; }

    public User()
    {
        Name = "";
        Id = 0;
    }

    public User(string name, int id)
    {
        Name = name;
        Id = id;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // * Collections in C#
        
        // * List
        List<int> list = new List<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.RemoveAt(0);
        Console.WriteLine(list);
        
        // * Dictionary
        Dictionary<string, int> dic = new Dictionary<string, int>();
        dic.Add("A", 1);
        dic.Add("B", 2);
        dic.Add("C", 3);
        dic.Remove("A");
        Console.WriteLine(dic);
        
        // * Stack
        Stack<int> stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        Console.WriteLine(stack);
        
        // * Queue
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Dequeue();
        Console.WriteLine(queue);
        
        // * HashSet
        HashSet<int> hs = new HashSet<int>();
        hs.Add(1);
        hs.Add(2);
        hs.Add(3);
        hs.Remove(1);
        Console.WriteLine(hs);
        
        // * Delegates
        MathematicalOperators math = new MathematicalOperators();

        // * File Handling in C#
        string filePath = "sample.txt";
        string binaryPath = "sample.bin";

        try
        {
            // * Writing
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Hello C#");
                writer.WriteLine("This is a file handling demo.");
            }

            Console.WriteLine("Text file written successfully");

            // * Reading
            using (StreamReader reader = new StreamReader(filePath))
            {
                Console.WriteLine("\nReading Text File :");
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            Console.WriteLine("\nData appended successfully");

            // * Appending a line
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Appended line added.");
            }

            Console.WriteLine("\nLine appended successfully");

            // * Checking File Existence
            if (File.Exists(filePath))
            {
                Console.WriteLine($"\nFile exists: {filePath}");
            }

            // * Fetching File Information
            FileInfo fileInfo = new FileInfo(filePath);
            Console.WriteLine("\nFile Information:");
            Console.WriteLine($"Name : {fileInfo.Name}");
            Console.WriteLine($"Size :  {fileInfo.Length} bytes");
            Console.WriteLine($"Created At : {fileInfo.CreationTime}");

            // * Binary File Write
            using (BinaryWriter bw = new BinaryWriter(File.Open(binaryPath, FileMode.Create)))
            {
                bw.Write(101);
                bw.Write("Binary File Handling");
                bw.Write(99.5);
            }

            Console.WriteLine("\nBinary file written successfully");

            // * Binary File Read
            using (BinaryReader br = new BinaryReader(File.Open(binaryPath, FileMode.Open)))
            {
                Console.WriteLine("\nReading Binary File");
                Console.WriteLine(br.ReadInt32());
                Console.WriteLine(br.ReadString());
                Console.WriteLine(br.ReadDouble());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.WriteLine("\nFile Handling Operation Completed Successfully!!!");
        }
        
        // * Lambda Expressions
        Func <int, int, int> sum = (a, b) => (a + b);
        Console.WriteLine(sum(10,12));
        
        Action<string> greet = name => Console.WriteLine("Hello " + name);
        greet("Shiv");
        
        Predicate<int> isEven = x => x % 2 == 0;
        Console.WriteLine(isEven(10));
        
        // * Serialisation
        User user = new User { Id = 1, Name = "Shiv" };

        XmlSerializer serializer = new XmlSerializer(typeof(User));

        using (FileStream fs = new FileStream("user.xml", FileMode.Create))
        {
            serializer.Serialize(fs, user);
        }

        using (FileStream fs = new FileStream("user.xml", FileMode.Open))
        {
            User u = (User)serializer.Deserialize(fs)!;
            Console.WriteLine(u.Name);
        }
    }
}