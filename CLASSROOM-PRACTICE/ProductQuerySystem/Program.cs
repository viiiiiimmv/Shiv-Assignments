namespace ProductQuerySystem;

using System;
using System.Collections.Generic;

class Program
{
    /*
    ==========================================================
    PRODUCT QUERY SYSTEM
    ==========================================================

    An e-commerce company stores product information.

    products[i] = [name, price, manufacturingYear]

    You must process queries of three types:

    Type1 -> Find products manufactured in a given year
    Type2 -> Find products with price LESS than a given value
    Type3 -> Find products with price GREATER than a given value

    Return product names in their ORIGINAL ORDER.

    ----------------------------------------------------------

    Example

    products =
    [
        ["key","50","2013"],
        ["fan","100","2012"],
        ["lock","150","2013"],
        ["table","200","2017"],
        ["toy","500","2017"]
    ]

    queries =
    [
        ["Type1","2013"],
        ["Type2","500"],
        ["Type3","500"],
        ["Type1","2017"]
    ]

    Output

    [
        ["key","lock"],
        ["key","fan","lock","table"],
        [],
        ["table","toy"]
    ]

    ----------------------------------------------------------

    Complete the function:

    public static List<List<string>> GetMatchingProducts(
        List<List<string>> products,
        List<List<string>> queries
    )
    */

    public static List<List<string>> GetMatchingProducts(
        List<List<string>> products,
        List<List<string>> queries)
    {
        List<List<string>> result = new List<List<string>>();

        // WRITE YOUR CODE HERE
        foreach (var query in queries)
        {
            string type = query[0];
            int val = int.Parse(query[1]);
            
            List<string> current = new List<string>();

            foreach (var product in products)
            {
                string name = product[0];
            }
        }

        return result;
    }

    static void Main()
    {
        List<List<string>> products = new List<List<string>>()
        {
            new List<string>{"key","50","2013"},
            new List<string>{"fan","100","2012"},
            new List<string>{"lock","150","2013"},
            new List<string>{"table","200","2017"},
            new List<string>{"toy","500","2017"}
        };

        List<List<string>> queries = new List<List<string>>()
        {
            new List<string>{"Type1","2013"},
            new List<string>{"Type2","500"},
            new List<string>{"Type3","500"},
            new List<string>{"Type1","2017"}
        };

        var ans = GetMatchingProducts(products, queries);

        foreach(var list in ans)
            Console.WriteLine(string.Join(", ", list));
    }
}