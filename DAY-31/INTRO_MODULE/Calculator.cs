namespace INTRO_MODULE;

public class Calculator
{
    public int Divide(int numerator, int denominator)
    {
        if (denominator == 0) 
        {
            throw new DivideByZeroException("Denominator cannot be zero.");
        }
        return numerator / denominator;
    }
}