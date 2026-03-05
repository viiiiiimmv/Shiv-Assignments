namespace TESTING_INTRODUCTION;

public class Calculate
{
    public int Addition(int num1, int num2)
    {
        return num1 + num2;
    }

    public int Subtraction(int num1, int num2)
    {
        return (num1 > num2) ? num1-num2 : num2-num1;
    }
    
    public int Multiplication(int num1, int num2)
    {
        return (num1*num2);
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
            Console.WriteLine(ex.Message);
        }
        
        return result;
    }

    public int PasswordStrength(string password)
    {
        if (string.IsNullOrEmpty(password)) return 0;

        int res = 0;
        if (password.Length > 8)
        {
            res = 1;
        }

        if (password.Any(char.IsUpper))
        {
            res = res + 1;
        }

        if (password.Any(char.IsLower))
        {
            res = res + 1;
        }

        if (password.Any(char.IsDigit))
        {
            res = res + 1;
        }
        
        string specialChars = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
        char[] specialArray = specialChars.ToCharArray();
        foreach (char c in specialArray)
        {
            if (password.Contains(c))
            {
                res = res + 1;
            }

        }
        return res;
    }
    
}