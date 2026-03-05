namespace CHAPTER_6_MSTESTS;

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

[TestClass]
public class CalculatorTests
{
    private Calculator _calculator;

    [TestInitialize]
    public void Init()
    {
        _calculator = new Calculator();
    }

    [TestMethod]
    public void Add_ReturnsCorrectSum()
    {
        int result = _calculator.Add(4, 6);

        Assert.AreEqual(10, result);
    }

    [DataTestMethod]
    [DataRow(1, 1, 2)]
    [DataRow(5, 5, 10)]
    public void Add_WorksForMultipleInputs(int a, int b, int expected)
    {
        Assert.AreEqual(expected, _calculator.Add(a, b));
    }
}