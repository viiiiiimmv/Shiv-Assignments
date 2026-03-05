// * LINQ PRACTICE DAY - 1

namespace DAY_1;

class Program
{
    static void Main(string[] args)
    {
        int[] numbers = [12, 33, 44, 55, 6, 78, 100, 289, 25, 90, 44, 12, 78, 55, 100];
        string[] names =
        [
            "Ravi", "Kiran", "Kishore", "Kavitha", "Mahesh", "Priya", "Suresh", "Anita", "Deepak", "Lakshmi", "Rajesh",
            "Pooja"
        ];

        // * ------------------------------------------------------------------------------------

        Console.WriteLine("====== NUMBER ARRAYS ======");
        // * Numbers greater than thirty
        // ! WHERE - used to select the values
        // ! SELECT - used to compare to return a bool result
        var numbersGreaterThan30 = numbers.Where(x => x > 30);
        foreach (var num in numbersGreaterThan30)
        {
            Console.Write(num + " ");
        }

        Console.WriteLine();

        // * Even numbers
        var evenNumbers = numbers.Where(x => x % 2 == 0);
        foreach (var num in evenNumbers)
        {
            Console.Write(num + " ");
        }

        Console.WriteLine();

        // * Finding the largest numbers and removing duplicates
        // ! DISTINCT - used to avoid duplicate values
        // ! TAKE - used to select a specific number of values
        var top3 = numbers.OrderByDescending(x => x).Distinct().Take(3);
        foreach (var num in top3)
        {
            Console.Write(num + " ");
        }

        Console.WriteLine();

        // * Group values based on the evens and odds. Find the count and average of each of them
        // ! GROUPBY - used to group values based on the criteria
        var evenOddGroups = numbers.GroupBy(x => x % 2 == 0 ? "EVEN NUMBERS" : "ODD NUMBERS");
        foreach (var group in evenOddGroups)
        {
            Console.WriteLine(group.Key);
            Console.WriteLine("-------------");
            Console.WriteLine("Group Size : " + group.Count());
            Console.WriteLine("Group Average : " + group.Average());
            Console.WriteLine("Group Items : " + string.Join(" ", group));
            Console.WriteLine();
        }

        // * Numbers should range from 20 to 100, then squared and sorted
        // ! SELECT - used to perform a specific operation on each element such as squaring the number
        var rangedSqauredSorted =
            numbers
                .Where(x => x is >= 20 and <= 100)
                .Distinct().Select(x => x * x)
                .OrderBy(x => x);
        Console.WriteLine(string.Join(" ", rangedSqauredSorted));

        // * Find most frequent elements
        var mostFrequent = numbers
            .GroupBy(x => x)
            .OrderByDescending(x => x.Count());
        foreach (var group in mostFrequent)
        {
            Console.WriteLine($"{group.Key} -> {group.Count()}");
        }

        Console.WriteLine();

        // * Cummalative sum 
        var cummalativeSum = numbers
            .Select((x, idx) => numbers.Take(idx + 1).Sum());
        Console.WriteLine(string.Join("\t", cummalativeSum));
        Console.WriteLine();

        Console.WriteLine("====== NAMES ARRAYS ======");

        // * Names starting with 'K'
        var namesStartingWithK = names
            .Where(x => x.StartsWith('K'));
        Console.WriteLine(string.Join(", ", namesStartingWithK));
        Console.WriteLine();

        // * Names with length
        foreach (var name in names)
        {
            Console.WriteLine($"{name}\t---> {name.Length}");
        }

        Console.WriteLine();

        // * Names with length > 5 and make it uppercase
        var namesGreaterThan5Upper = names
            .Where(x => x.Length > 5)
            .Select(x => x.ToUpper());
        Console.WriteLine(string.Join(", ", namesGreaterThan5Upper));
        Console.WriteLine();
        
        // * Names ending with a vowel and sort them by length in descending order
        var namesEndingWithVowel = names
            .Where(x => "aeiouAEIOU".Contains(x[^1]))
            .OrderByDescending(x => x.Length);
        
        Console.WriteLine(string.Join(", ", namesEndingWithVowel));
        Console.WriteLine();
        
        // * Group by length and get the shortest and longest
        var groupNamesbyLength = names.GroupBy(x => x.Length);
        foreach (var group in groupNamesbyLength)
        {
            var shortestName = group.OrderBy(x => x).First();
            var longestName = group.OrderByDescending(x => x).First();
            Console.WriteLine($"{shortestName} -> {shortestName.Length}");
            Console.WriteLine($"{longestName} -> {longestName.Length}");
        }
        Console.WriteLine();
        
        // * Reverse each name and concatenate with a '-'
        var reversedConcatednatedName = names.Select(x => new string(x.Reverse().ToArray())).Select(x => x+"-");
        Console.WriteLine(string.Join(" ", reversedConcatednatedName));
        
        // * Names containing 'a' or 'i' , count chars except vowels
        var nonVowelWords = names
            .Where(x => x.ToLower().Contains('a') || x.ToLower().Contains('i'));

        foreach (var word in nonVowelWords)
        {
            int count = 0;
            foreach (var letter in word)
            {
                if (!"aeiouAEIOU".Contains(letter))
                {
                    count++;
                }
            }
            Console.WriteLine($"{word}\t--> {count}");
        }
        
        // * First three names, alphabetically ordered , joined uppercase
        var firstThreeAlphabeticallyUpper = string.Join("",names.OrderBy(x => x).Take(3).Select(x => x.ToUpper()));
        Console.WriteLine(firstThreeAlphabeticallyUpper);
        
        // * Get all names with length > 3 using any and all
        // ! ALL - used to select all elements when they fulfil a certain condition
        Console.WriteLine(string.Join(" ",names.Where(name => name.Take(4).All(char.IsLetter))));
        
        // * Any name that starts with P
        // ! ANY - used to select any value from the elements that satisfies the condition
        Console.WriteLine(names.Any( x => x.StartsWith('P')));


    }
}