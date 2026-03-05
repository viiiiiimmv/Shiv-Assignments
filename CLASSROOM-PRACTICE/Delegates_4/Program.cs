namespace Delegates_4;

using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
ROYAL BANK FRAUD DETECTION SYSTEM
==========================================================

STORY
The Royal Bank manages thousands of accounts and
millions of transactions across the kingdom.

To detect fraud and generate financial insights,
the bank needs a flexible system using:

- OOP
- Delegates
- LINQ
- Functional Programming

Your task is to design the system.

----------------------------------------------------------
TASKS TO IMPLEMENT
----------------------------------------------------------

1️⃣ Create an abstract class Account

Properties
- string AccountNumber
- double Balance

Method
- abstract double CalculateInterest()

RULES

SavingsAccount
Interest = Balance * 0.04

BusinessAccount
Interest = Balance * 0.06
*/

public abstract class Account
{
    public string? AccountNumber { get; set; }
    public int Balance { get; set; }

    public abstract double CalculateInterest();
}
/*

----------------------------------------------------------

2️⃣ Create derived classes

SavingsAccount
BusinessAccount

Both must override CalculateInterest()

*/
public class SavingsAccount : Account
{
    public override double CalculateInterest()
    {
        return Balance * 0.04;
    }
}

public class BusinessAccount : Account
{
    public override double CalculateInterest()
    {
        return Balance * 0.06;
    }
}
/*
----------------------------------------------------------

3️⃣ Create class Transaction

Properties

- int TransactionId
- string AccountNumber
- double Amount
- string Type   ("Credit" or "Debit")
- DateTime Date

*/
public class Transaction
{
    public int TransactionId { get; set; }
    public string AccountNumber { get; set; }
    public double Amount { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
}

public delegate bool TransactionFilter(Transaction t);
/*
----------------------------------------------------------

4️⃣ Create delegate

public delegate bool TransactionFilter(Transaction t);

This delegate allows dynamic transaction filtering.


----------------------------------------------------------
*/
public class BankSystem
{
    private List<Account> _accounts = new List<Account>();
    private List<Transaction> _transactions = new List<Transaction>();

    public void AddAccount(Account account)
    {
        _accounts.Add(account);
    }

    public void AddTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
    }

    public List<Transaction> GetTransactions()
    {
        return _transactions;
    }

    public List<Account> GetAccounts()
    {
        return _accounts;
    }

    public List<Transaction> FilterTransactions(TransactionFilter filter)
    {
        return _transactions.Where(t => filter(t)).ToList();
    }
    
    public IEnumerable<(string Key, double Value)> GenerateReport(
        Func<Transaction, bool> filter,
        Func<Transaction, string> groupBy,
        Func<IEnumerable<Transaction>, double> aggregate)
    {
        return _transactions
            .Where(filter)
            .GroupBy(groupBy)
            .Select(g => (Key: g.Key, Value: aggregate(g)));
    }
}
/*
5️⃣ Create class BankSystem

Field
- private List<Account> accounts
- private List<Transaction> transactions

Methods to implement

AddAccount(Account account)

AddTransaction(Transaction transaction)

GetTransactions()

GetAccounts()


----------------------------------------------------------

6️⃣ Implement method

FilterTransactions(TransactionFilter filter)

RULES
- Use LINQ Where
- Return filtered transactions


Example usage
FilterTransactions(t => t.Amount > 50000)


----------------------------------------------------------

7️⃣ Implement method

GenerateReport(
    Func<Transaction,bool> filter,
    Func<Transaction,string> groupBy,
    Func<IEnumerable<Transaction>,double> aggregate)

RULES

Step 1
Filter transactions

Step 2
Group using groupBy

Step 3
Aggregate each group

Return a collection containing

GroupKey
ResultValue


Example use cases

Monthly totals
Account totals
Fraud analysis


----------------------------------------------------------

8️⃣ LINQ REPORT REQUIREMENTS

Implement these reports

A) Total credits per account

Rules
Type == "Credit"
GroupBy AccountNumber
Sum Amount


B) Suspicious transactions

Rules
Amount > 50000


C) Monthly transaction summary

Rules
Group by month
Sum transaction amounts


----------------------------------------------------------

9️⃣ DO NOT MODIFY MAIN()

Main assumes all required classes and
methods are correctly implemented.

==========================================================
*/

class Program
{
    static void Main()
    {
        BankSystem bank = new BankSystem();

        //--------------------------------------------------
        // CREATE ACCOUNTS
        //--------------------------------------------------

        Account a1 = new SavingsAccount
        {
            AccountNumber = "A100",
            Balance = 80000
        };

        Account a2 = new BusinessAccount
        {
            AccountNumber = "B200",
            Balance = 250000
        };

        Account a3 = new SavingsAccount
        {
            AccountNumber = "A300",
            Balance = 45000
        };

        bank.AddAccount(a1);
        bank.AddAccount(a2);
        bank.AddAccount(a3);

        //--------------------------------------------------
        // CREATE TRANSACTIONS
        //--------------------------------------------------

        List<Transaction> transactions = new List<Transaction>
        {
            new Transaction { TransactionId = 1, AccountNumber="A100", Amount=20000, Type="Credit", Date = new DateTime(2026,1,10) },
            new Transaction { TransactionId = 2, AccountNumber="A100", Amount=15000, Type="Debit", Date = new DateTime(2026,1,11) },
            new Transaction { TransactionId = 3, AccountNumber="B200", Amount=120000, Type="Credit", Date = new DateTime(2026,2,3) },
            new Transaction { TransactionId = 4, AccountNumber="A300", Amount=8000, Type="Debit", Date = new DateTime(2026,2,12) },
            new Transaction { TransactionId = 5, AccountNumber="B200", Amount=60000, Type="Debit", Date = new DateTime(2026,3,5) },
            new Transaction { TransactionId = 6, AccountNumber="A100", Amount=5000, Type="Credit", Date = new DateTime(2026,3,18) },
            new Transaction { TransactionId = 7, AccountNumber="A300", Amount=90000, Type="Credit", Date = new DateTime(2026,4,1) }
        };

        foreach (var t in transactions)
        {
            bank.AddTransaction(t);
        }

        //--------------------------------------------------
        // FRAUD FILTER USING DELEGATE
        //--------------------------------------------------

        Console.WriteLine("\nSUSPICIOUS TRANSACTIONS (> 50000)");

        TransactionFilter fraudFilter = t => t.Amount > 50000;

        var suspicious = bank.FilterTransactions(fraudFilter);

        foreach (var t in suspicious)
        {
            Console.WriteLine($"Tx {t.TransactionId} | {t.AccountNumber} | {t.Amount}");
        }

        //--------------------------------------------------
        // TOTAL CREDITS PER ACCOUNT
        //--------------------------------------------------

        Console.WriteLine("\nTOTAL CREDITS PER ACCOUNT");

        var creditReport = bank.GenerateReport(
            t => t.Type == "Credit",
            t => t.AccountNumber,
            g => g.Sum(x => x.Amount)
        );

        foreach (var r in creditReport)
        {
            Console.WriteLine($"{r.Key} -> {r.Value}");
        }

        //--------------------------------------------------
        // MONTHLY SUMMARY
        //--------------------------------------------------

        Console.WriteLine("\nMONTHLY TRANSACTION SUMMARY");

        var monthlyReport = bank.GenerateReport(
            t => true,
            t => t.Date.Month.ToString(),
            g => g.Sum(x => x.Amount)
        );

        foreach (var r in monthlyReport)
        {
            Console.WriteLine($"Month {r.Key} -> {r.Value}");
        }

        //--------------------------------------------------
        // INTEREST CALCULATIONS
        //--------------------------------------------------

        Console.WriteLine("\nINTEREST CALCULATIONS");

        foreach (var acc in bank.GetAccounts())
        {
            Console.WriteLine($"{acc.AccountNumber} Interest -> {acc.CalculateInterest()}");
        }
    }
}