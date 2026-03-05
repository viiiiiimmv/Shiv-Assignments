namespace CHAPTER_6_XUNIT;

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

public class UnitTest1
{
    [Fact]
    public void Add_ReturnsCorrectSum()
    {
        var calculator = new Calculator();

        int result = calculator.Add(2, 3);

        Assert.Equal(5, result);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(5, 5, 10)]
    public void Add_WorksForMultipleInputs(int a, int b, int expected)
    {
        var calculator = new Calculator();

        Assert.Equal(expected, calculator.Add(a, b));
    }
}