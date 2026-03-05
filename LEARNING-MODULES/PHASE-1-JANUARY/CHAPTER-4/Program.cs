namespace CHAPTER_4;

// Classes and Objects
class Student
{
    public string Name;
    public int Id;
    public int Age;

    public Student(string name, int id, int age)
    {
        Name = name;
        Id = id;
        Age = age;
    }
}

// Encapsulation / Data Hiding

class BankAccount
{
    private double _balance;

    public double Balance
    {
        get { return _balance; }
        set
        {
            if (value >= 0)
            {
                _balance = value;
            }
        }
    }
}

// Abstraction

// Abstract Classes
abstract class Shape
{
    public abstract double Area();
}

class Square : Shape
{
    private double _width;

    public override double Area()
    {
        return _width * _width;
    }
}

// Interfaces
interface IAnimal
{
    void Speak();
}

class Lion : IAnimal
{
    public void Speak()
    {
        Console.WriteLine("Roar");
    }
}

// Inheritance
class Vehicle
{
    public void Start()
    {
        Console.WriteLine("Vehicle Start!");
    }
}

class Car : Vehicle
{
    public void Drive()
    {
        Console.WriteLine("Car Drive!");
    }
}

// Polymorphism

// Method Overloading
class Calculator
{
    public int Add(int x, int y)
    {
        return x + y;
    }

    public double Add(double x, double y)
    {
        return x + y;
    }
}

// Method Overriding
class Parent
{
    public void Greet()
    {
        Console.WriteLine("Namaste");
    }
}

class Child : Parent
{
    public override void Greet()
    {
        Console.WriteLine("Hola");
    }
}

// Constructors and Destructors
class Class
{
    public Class()
    {
        Console.WriteLine("Object Created");
    }

    ~Class()
    {
        Console.WriteLine("Object deleted");
    }
}

// Static Keyword : Belongs to the class not the object
class Counter
{
    public static int count = 0;
}

// Sealed Class : Prevents inheritance or overriding
sealed class FinalClass
{
    // Code goes here
}

// IS A - inheritance , HAS A - composition
// class Dog : Animal { }

// class Engine{}
// class Car { Engine e = new Engine(); }


// Struct 
struct Point
{
    private int x;
    private int y;
    
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

// Enums : set of named constants
enum Days
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

enum Progress : byte
{
    Success = 10,
    Failure = 0,
    Pending = 1
}

// Access Modifiers

// public - everywhere
// private - class only
// protected - same class + derived class
// internal - same assembly
// protected internal - same assembly or derived classes
// private protected - same assembly and derived classes


// Operator Overloading

class Coordinate
{
    public int x, y;
    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Coordinate operator +(Coordinate p1, Coordinate p2)
    {
        return new Coordinate(p1.x + p2.x, p1.y + p2.y);
    }
}

// Method Hiding

class Layer_1
{
    public void Show()
    {
        Console.WriteLine("Layer 1");
    }
}

class Layer_2 : Layer_1
{
    public new void Show()
    {
        Console.WriteLine("Layer 2");
    }
}

// Layer_1 obj1 = new Layer_2(); -- Layer_1 Show Method
// Layer_2 obj2 = new Layer_2(); -- Layer_2 Show Method

class Program
{
    static void Main(string[] args)
    {
        Student s1 = new Student("Shiv",15150,21);
        Days day = Days.Monday;
        
        Progress pg =  Progress.Success;
        switch (pg)
        {
            case Progress.Success:
                Console.WriteLine("Success");
                break;
            
            case Progress.Failure:
                Console.WriteLine("Failure");
                break;
            
            case Progress.Pending:
                Console.WriteLine("Pending");
                break;
        }

    }
}