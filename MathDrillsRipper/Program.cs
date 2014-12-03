using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MathDrillsRipper.Properties;

namespace MathDrillsRipper
{
    class Program
    {
        private static void Main(string[] args)
        {
            string baseUrl = Settings.Default.TargetUrl;

            var console = new Console();
            var queue = new CrawlQueue(console);
            queue.AddUrl("/");

            using (var pdfWriter = new FileListWriter(Settings.Default.Pdfs, console))
            {
                List<Task> tasks = new List<Task>();

                Task anchorParser = Task.Factory.StartNew(() => Run(baseUrl, queue, pdfWriter, console));
                tasks.Add(anchorParser);

                //if (!Debugger.IsAttached)
                //{
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(6)); // ramp up slowly
                        console.WriteWarning("------------------ Starting up thread no.{0}", (i + 2));
                        tasks.Add(Task.Factory.StartNew(() => Run(baseUrl, queue, pdfWriter, console)));
                    }
                //}

                Task.WaitAll(tasks.ToArray());
            }

            console.WriteInfo("All done");
            System.Console.ReadKey();
        }

        private static void Run(string baseUrl, CrawlQueue queue, FileListWriter pdfWriter, Console console)
        {
            var snatcher = new Snatcher(baseUrl, console);

            Url url;
            while ((url = queue.GetNext()) != null)
            {
                Page page = snatcher.GetPage(url);

                if (page != null)
                {
                    foreach (string anchor in page.FindLocalPages(baseUrl))
                    {
                        queue.AddUrl(anchor);
                    }

                    foreach (string pdf in page.FindPdfs(baseUrl))
                    {
                        pdfWriter.WriteEntry(baseUrl, pdf);
                    }
                }
            }
        }
    }
}
