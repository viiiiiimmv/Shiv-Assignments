namespace INTRO_MODULE;

public class BankAccount
{
    public int Balance;

    public BankAccount()
    {
        Balance = 0;
    }
    public void Deposit(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Enter a value greater than zero");
        }
        Balance += amount;
        
    }

    public void Withdraw(int amount)
    {
        if (Balance - amount < 0)
        {
            throw new ArgumentException("Insufficient funds");
        }
        Balance -= amount;
    }
}