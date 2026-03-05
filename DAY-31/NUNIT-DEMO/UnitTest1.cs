namespace NUNIT_DEMO;
using INTRO_MODULE;
using NUnit.Framework;
using System;

[TestFixture]
public class CalculatorTests
{
    private Calculator _calculator;

    [SetUp]
    public void Setup()
    {
        _calculator = new Calculator();
    }

    // Positive test case: normal division
    [Test]
    public void Divide_ShouldReturnCorrectResult_WhenInputsAreValid()
    {
        var result = _calculator.Divide(10, 2);
        Assert.AreEqual(5, result);
    }

    // Negative test case: dividing by zero
    [Test]
    public void Divide_ShouldThrowDivideByZeroException_WhenDenominatorIsZero()
    {
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
    }

    // Edge case: division resulting in a fraction (rounding down)
    [Test]
    public void Divide_ShouldRoundDown_WhenResultIsFraction()
    {
        var result = _calculator.Divide(5, 2);
        Assert.AreEqual(2, result); // Integer division, rounds down
    }

    // Edge case: very large numbers
    [Test]
    public void Divide_ShouldHandleLargeNumbers()
    {
        var result = _calculator.Divide(int.MaxValue, 1);
        Assert.AreEqual(int.MaxValue, result);
    }
}