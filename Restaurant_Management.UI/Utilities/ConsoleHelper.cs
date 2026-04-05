namespace Restaurant_Management.UI.Utilities;

public static class ConsoleHelper
{
    public static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 60));
        Console.WriteLine(title.PadLeft(title.Length + (60 - title.Length) / 2));
        Console.WriteLine(new string('=', 60));
    }

    public static void PrintSubHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', 60));
        Console.WriteLine(title);
        Console.WriteLine(new string('-', 60));
    }

    public static void PrintSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {message}");
        Console.ResetColor();
    }

    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ {message}");
        Console.ResetColor();
    }

    public static void PrintWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"⚠ {message}");
        Console.ResetColor();
    }

    public static void PrintInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"ℹ {message}");
        Console.ResetColor();
    }

    public static string? ReadString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int result))
                return result;

            PrintError("Please enter a valid integer.");
        }
    }

    public static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal result))
                return result;

            PrintError("Please enter a valid decimal number.");
        }
    }

    public static void PrintTable<T>(IEnumerable<T> items, params (string Header, Func<T, string> Accessor)[] columns)
    {
        var itemList = items.ToList();
        if (!itemList.Any())
        {
            PrintInfo("No items to display.");
            return;
        }

        var colWidths = columns.Select(c => Math.Max(c.Header.Length, itemList.Max(i => c.Accessor(i).Length))).ToList();

        Console.WriteLine();
        for (int i = 0; i < columns.Length; i++)
        {
            Console.Write(columns[i].Header.PadRight(colWidths[i] + 2));
        }
        Console.WriteLine();

        Console.WriteLine(string.Join("", Enumerable.Range(0, colWidths.Sum() + columns.Length * 2).Select(_ => "-")));

        foreach (var item in itemList)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                Console.Write(columns[i].Accessor(item).PadRight(colWidths[i] + 2));
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
