using System;
using System.Collections.Generic;

class Program
{
    static void FindDuplicates(string[] arr)
    {
        List<string> seen = new List<string>();
        List<string> duplicate = new List<string>();

        foreach (string item in arr)
        {
            if (seen.Contains(item) && !duplicate.Contains(item))
            {
                duplicate.Add(item);
            }
            else
            {
                seen.Add(item);
            }
        }

        Console.WriteLine("Duplicate elements: " + string.Join(", ", duplicate));
    }

    public static void Main()
    {
        string[] arr = { "month", "day", "year", "day", "month" };
        FindDuplicates(arr);
    }
}
