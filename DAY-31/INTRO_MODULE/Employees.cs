namespace INTRO_MODULE;

public class CheckEmployee
{
    public virtual Boolean CheckEMP()
    {
        throw new NotImplementedException();
    }

    public virtual int Subtract(int num1, int num2)
    {
        throw new NotImplementedException();
    }
}

public class ProcessEmployee
{
    public bool InsertEmployee(CheckEmployee emp)
    {
        emp.CheckEMP();
        return true;
    }

    public int Subtract(CheckEmployee emp)
    {
        return emp.Subtract(4, 3);
    }
}