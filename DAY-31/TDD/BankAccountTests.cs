using INTRO_MODULE;

namespace TDD;
using INTRO_MODULE;

[TestFixture]
public class BankAccountTests
{
    [Test]
    public void DepositShouldIncrease()
    {

        var account = new BankAccount();
        account.Deposit(100);
        Assert.AreEqual(100, account.Balance);
    }

    [Test]
    public void WithdrawShouldDecrease()
    {
        var account = new BankAccount();
        account.Deposit(100);
        account.Withdraw(50);
        Assert.AreEqual(50, account.Balance);
    }

    [Test]
    public void InsufficientFundsShouldThrow()
    {
        var account = new BankAccount();
        account.Withdraw(50);
        Assert.Throws<InvalidOperationException>(() => account.Withdraw(100));
    }
}