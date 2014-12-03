using System;

namespace MathDrillsRipper
{
    public class Console : IConsole
    {
        public void WriteInfo(string format, params object[] values)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(format, values);
            System.Console.ResetColor();
        }

        public void WriteWarning(string format, params object[] values)
        {
            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.WriteLine(format, values);
            System.Console.ResetColor();
        }

        public void WriteError(string format, params object[] values)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(format, values);
            System.Console.ResetColor();
        }
    }
}