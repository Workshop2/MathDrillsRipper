using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MathDrillsRipper
{
    public class Console : IConsole
    {
        private readonly Queue<ConsoleMessage> _messageQueue = new Queue<ConsoleMessage>();
        private readonly object _lock = new object();

        public Console()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    ConsoleMessage message;
                    lock (_lock)
                    {
                        message = _messageQueue.Any() ? _messageQueue.Dequeue() : null;
                    }

                    do
                    {
                        if (message != null)
                        {
                            System.Console.ForegroundColor = message.Colour;

                            if (message.Values.Any())
                            {
                                System.Console.WriteLine(message.Format, message.Values);
                            }
                            else
                            {
                                System.Console.WriteLine(message.Format);
                            }

                            System.Console.ResetColor();
                        }


                        lock (_lock)
                        {
                            message = _messageQueue.Any() ? _messageQueue.Dequeue() : null;
                        }
                    } while (message != null);

                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                }
            });
        }

        public void WriteInfo(string format, params object[] values)
        {
            lock (_lock)
            {
                _messageQueue.Enqueue(new ConsoleMessage { Format = format, Values = values.ToArray(), Colour = ConsoleColor.Green });
            }
        }

        public void WriteWarning(string format, params object[] values)
        {
            lock (_lock)
            {
                _messageQueue.Enqueue(new ConsoleMessage { Format = format, Values = values.ToArray(), Colour = ConsoleColor.Magenta });
            }
        }

        public void WriteError(string format, params object[] values)
        {
            lock (_lock)
            {
                _messageQueue.Enqueue(new ConsoleMessage { Format = format, Values = values, Colour = ConsoleColor.Red });
            }
        }
    }
}