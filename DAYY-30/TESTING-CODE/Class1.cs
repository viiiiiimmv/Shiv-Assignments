namespace TESTING_CODE;

public class Calculate 
{
    public int Addition(int num1,int num2)
    {
        return num1 + num2;
    }
    public int substract(int num1,int num2)
    {
        int result;
        if(num1>num2)
        {
            result = num1 - num2;
        }
        else
        {
            result = num2 - num1;
        }
        return result;
    }
    public int Multiplication(int num1, int num2)
    {
        return num1 * num2;
    }

    public int Divide(int num1, int num2)
    {
        int result = 0;
        try
        {
            result = num1 / num2;
        }
        catch (DivideByZeroException ex)
        {
            throw ex;
        }
        return result;


    }

    public int GetPassWordStrength(string password)
    {
        if(string.IsNullOrEmpty(password))
        {
            return 0;
        }
        int result = 0;
        if(password.Length > 8)
        {
            result = result + 1;
        }
        if(password.Any(char.IsUpper))
        {
            result = result + 1;
        }

        if (password.Any(char.IsLower))
        {
            result = result + 1;
        }

        string specialchars = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
        char[] specarrray = specialchars.ToCharArray();
        foreach (char c in specarrray)
        {
            if (password.Contains(c))
            {
                result = result + 1;
            }

        }

        if (password.Any(char.IsDigit))
        {
            result = result + 1;
        }

        return result;
    }
}
