namespace QUESTION_2;

using System;
using System.Collections.Generic;

/* ================= INTERFACE ================= */

public interface IPayment
{
    string PaymentId { get; }
    double Amount { get; }

    double ProcessPayment();
}

/* =================================================
   IMPLEMENT BELOW CLASSES

   class CreditCardPayment : IPayment
   class UpiPayment : IPayment

   Rules:
   - CreditCardPayment charges 2% processing fee
   - UpiPayment charges 1% processing fee
   ================================================= */

public class CreditCardPayment : IPayment
{
    public string PaymentId { get; }
    public double Amount { get; set; }

    public CreditCardPayment(string paymentId, double amount)
    {
        PaymentId = paymentId;
        Amount = amount;
    }

    public double ProcessPayment()
    {
        return Amount * 1.02;
    }
}

public class UpiPayment : IPayment
{
    public string PaymentId { get; set; }
    public double Amount { get; set; }

    public UpiPayment(string paymentId, double amount)
    {
        PaymentId = paymentId;
        Amount = amount;
    }

    public double ProcessPayment()
    {
        return Amount * 1.01;
    }
}

class Program
{
    static void Main()
    {
        List<IPayment> payments = new List<IPayment>
        {
            new CreditCardPayment("P101", 10000),
            new UpiPayment("P102", 5000),
            new CreditCardPayment("P103", 2000)
        };

        foreach (var p in payments)
        {
            Console.WriteLine($"{p.PaymentId} Final Amount: {p.ProcessPayment()}");
        }
    }
}