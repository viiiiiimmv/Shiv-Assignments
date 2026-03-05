using System.Linq.Expressions;

namespace SEAT_BOOKING_SYSTEM;

public class SeatNotAvailableException : Exception
{
    public SeatNotAvailableException(string message) : base(message){}
}

class Program
{
    public static int[] Seats = new int[50];
    
    public static void printBookedSeats()
    {
        Console.WriteLine("Following Seat have been already booked :");
        for (int i = 0; i < 50; i++)
        {
            if (Seats[i] == 1)
            {
                Console.Write($"{i}\t");
            }
        }
        Console.WriteLine();
    }

    public static int EmptySeats()
    {
        int count = 0;
        foreach (int i in Seats)
        {
            if (i == 0)
            {
                count++;
            }
        }

        return count;
    }
    
    static void Main(string[] args)
    {
        bool response = true;
        do
        {
            try
            {
                Console.WriteLine($"Available seats: {EmptySeats()}");
                Console.Write("Enter seat number which you wish to book : ");
                int seat = Convert.ToInt32(Console.ReadLine());

                if (seat < 0 || seat >= 50)
                {
                    Console.WriteLine("Invalid seat number");
                    Console.Write("Want to select a different seat number? (y/n) ");
                    string answer = Console.ReadLine();
                    if (answer.ToLower() == "y")
                    {
                        seat = Convert.ToInt32(Console.ReadLine());
                    }
                    else
                    {
                        response = false;
                    }
                }

                if (Seats[seat] == 1)
                {
                    throw new SeatNotAvailableException(
                        $"Seat {seat} is already booked."
                    );
                }

                Seats[seat] = 1;
                Console.WriteLine($"Seat {seat} has been successfully booked");
                Console.WriteLine("*********------------*********");
                Console.WriteLine("1. Book Another Seat\n2.Display all booked seats\n3.Exit");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        continue;
                        break;
                    case 2:
                        printBookedSeats();
                        break;
                    case 3:
                        Console.WriteLine(("Thankyou!!"));
                        response = false;
                        break;
                    default:
                        Console.WriteLine(("Invalid choicer"));
                        break;
                }

            }
            catch (SeatNotAvailableException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        } while (response);
    }
}