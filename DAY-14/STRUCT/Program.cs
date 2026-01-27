namespace STRUCT
{
    public struct Student
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public double[] Marks { set; get; }

        public Student(int id, string name, int numberofsubjects)
        {
            ID = id;
            Name = name;
            Marks = new double[numberofsubjects];
        }
        // adding marks to array of particular student
        public void AddMarks(double[] marks)
        {
            if (marks.Length != Marks.Length)
            {
                Console.WriteLine("Cannot add marks as marks count is not matching..");

            }
            else
            {
                for (int i = 0; i < Marks.Length; i++)
                {
                    Marks[i] = marks[i];
                }

            }
            Console.WriteLine($"Marks added to {Name}");
        }

        public double CalculateAvergageMark()
        {
            double sum = 0;
            foreach (double mark in Marks)
            {
                sum += mark;

            }
            return Marks.Length > 0 ? sum / Marks.Length : 0;
        }

        public char DetermineGrade()
        {
            double average = CalculateAvergageMark();
            if (average > 80)
            {
                return 'A';
            }

            else if (average > 70)
            {
                return 'B';
            }
            else if (average > 60)
            {
                return 'C';
            }
            else if (average > 40)
            {
                return 'D';
            }
            else
            {
                return 'F';
            }
        }
        public void displaydetails()
        {
            Console.WriteLine($"ID:{ID},Name :{Name}");
            Console.WriteLine("Marks:");
            foreach (double mark in Marks)
            {
                Console.Write($"\t{mark}");
            }

            Console.WriteLine($"\nAvergae marks :{CalculateAvergageMark():F2}");
            Console.WriteLine($"\nGrade: {DetermineGrade()}");
        }

    }
    class Program
    {
        static void Main(string[] args)
        {

            Student student1 = new Student(1, "ravi", 3);
            student1.AddMarks(new double[] { 86.89, 69.78, 90.67 });
            student1.CalculateAvergageMark();
            student1.DetermineGrade();
            student1.displaydetails();
            Console.ReadLine();

            // structures cannot be inherited
            // encapsulation is not possible here
            // private protected doesn't work here
            // Access Specifiers : public, private
            // inheritance is not supported
            // polymorphism is supported8
        }
    }
}
