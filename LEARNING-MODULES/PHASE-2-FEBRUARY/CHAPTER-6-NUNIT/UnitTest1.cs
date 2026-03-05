using NUnit.Framework;
namespace CHAPTER_6_NUNIT;

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

[TestFixture]
public class CalculatorTests
{
    private Calculator _calculator;

    [SetUp]
    public void Setup()
    {
        _calculator = new Calculator();
    }

    [Test]
    public void Add_ReturnsCorrectSum()
    {
        int result = _calculator.Add(3, 4);

        Assert.Equals(7, result);
    }

    [TestCase(2, 3, 5)]
    [TestCase(10, 20, 30)]
    public void Add_WorksForMultipleInputs(int a, int b, int expected)
    {
        Assert.Equals(expected, _calculator.Add(a, b));
    }
}