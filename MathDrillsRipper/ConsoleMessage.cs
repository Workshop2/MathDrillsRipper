using System;

namespace MathDrillsRipper
{
    public class ConsoleMessage
    {
        public string Format { get; set; }
        public object[] Values { get; set; }
        public ConsoleColor Colour { get; set; }
    }
}