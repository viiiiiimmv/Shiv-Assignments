using System;
namespace ExpressionBodied
{
    public class BankAccount
    {
        public string AccountNumber { get; }
        public decimal Balance { get; private set; }
        public string AccountHolder { get; set; }

        // Traditional Constructor
        public BankAccount(string accountNumber, string accountHolder, decimal initialBalance)
            => (AccountNumber, AccountHolder, Balance) = (accountNumber, accountHolder, initialBalance >= 0 ? initialBalance : 0);

        // Traditional Destructor/Finalizer
        ~BankAccount()=>Console.WriteLine($"Account {AccountNumber} finalized - logging closure");

        // Traditional Methods
        public bool Deposit(decimal amount) => (amount > 0) && (Balance += amount) > 0 ? true : false;

        public bool Withdraw(decimal amount) => (amount > 0) && (Balance -= amount) >= 0 ? true : false;

        public string GetAccountStatus() => Balance > 1000 ? "Premium" : "Standard";

        //public decimal CalculateInterestRate()
        //{
        //    return Balance switch
        //    {
        //        > 10000 => 0.05m,
        //        > 5000 => 0.04m,
        //        _ => 0.02m
        //    };
        //}

        // Traditional Property
        public string Summary => $"Account: {AccountNumber}, Balance: ${Balance:N2}";
    }

    internal class Program
    {
        public static void Main()
        {
            var account = new BankAccount("ACC123", "John Doe", 5000);
            account.Deposit(2500);
            Console.WriteLine(account.Summary);
            Console.WriteLine($"Status: {account.GetAccountStatus()}");
        }
    }
}
