namespace CHAPTER_2;

class Program
{
    static void Main(string[] args)
    {
        //  Arrays : Linear data structure used to store data of same type (primitive)
        
        // Linear array of size 5 
        int[] arr1 = new int[5];
        arr1 = new int[] {1,2,3,4,5};
        foreach (int item in arr1)
        {
            Console.WriteLine(item);
        }
        
        // Multidimensional Arrays : All rows have same size
        int[,] arr2 =  new int[5, 5];
        
        // Jagged Arrays : Rows can have different sizes
        int[][] arr3 = new int[5][];
        
        // IS keyword : used to check the datatype of the given object
        object obj1 = "Hello World";
        if (obj1 is string)
        {
            Console.WriteLine((string)obj1);
        }
        
        // AS keyword : used to cast a given object to specific datatype
        object obj2 = "Hello World";
        string? str1 = obj2 as string;
        Console.WriteLine(str1);
    }
    
    // REF keyword : passes a value by reference, must be initialized before passing
    // method can read and modify values
    void Increment(ref int x)
    {
        x += 1;
    }
    
    // OUT keyword : passes a value by reference, doesn't need initialization
    // method must assign a value before returning
    bool Divide(int a, int b, out int res)
    {
        if (b == 0)
        {
            res = 0;
            return false;
        }

        res = b / a;
        return true;
    }
}