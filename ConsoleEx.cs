public static class ConsoleEx
{
    public static void Red(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Blue(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Blue;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Green(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Cyan(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Magenta(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Magenta;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Yellow(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }

    public static void WriteLine(string output, ConsoleColor color = ConsoleColor.White)
    {
        System.Console.ForegroundColor = color;
        Console.WriteLine(output);
        System.Console.ResetColor();
    }
    public static void Write(string output, ConsoleColor color = ConsoleColor.White)
    {
        System.Console.ForegroundColor = color;
        Console.Write(output);
        System.Console.ResetColor();
    }

    public static string? ReadLine(string lineToDisplay = "", ConsoleColor color = ConsoleColor.White)
    {
        System.Console.ForegroundColor = color;
        System.Console.WriteLine(lineToDisplay);
        System.Console.ResetColor();
        return Console.ReadLine();
    }
    public static int ReadInt(string lineToDisplay = "", string errorMessage = "", ConsoleColor color = ConsoleColor.White, ConsoleColor errorColor = ConsoleColor.Red)
    {

        System.Console.ForegroundColor = color;
        System.Console.WriteLine(lineToDisplay);
        System.Console.ResetColor();
        var line = Console.ReadLine();
        int result;
        while (!int.TryParse(line, out result))
        {
            //then display error message
            System.Console.ForegroundColor = errorColor;
            System.Console.WriteLine(lineToDisplay);
            System.Console.ResetColor();

            line = Console.ReadLine();
        }

        return result;
    }

    private static int waitingAnimIndex;
    private static char[] waitingAnim = new char[] { '|', '\\', '-', '/' };
    private static char waitingCursor = '|';
    public static void DrawWaitingThing(string extra = "")
    {
        var pos = Console.GetCursorPosition();
        waitingAnimIndex = (waitingAnimIndex + 1) % waitingAnim.Length;
        waitingCursor = waitingAnim[waitingAnimIndex];
        Console.SetCursorPosition(20, 0);
        Console.Write($"{waitingCursor} {extra}");
        Console.SetCursorPosition(pos.Left, pos.Top);
    }
}