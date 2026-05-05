Console.WriteLine($"{(10 / 3.0):F2}"); 

Console.Write("Enter a number : ");
var num = Convert.ToInt32(Console.ReadLine());

for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"{num,2} x {i,2} = {num * i,3}");
}