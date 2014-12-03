using System.Threading.Tasks;
using MathDrillsRipper.Properties;

namespace MathDrillsRipper
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUrl = Settings.Default.TargetUrl;

            var console = new Console();
            var queue = new CrawlQueue(console);
            queue.AddUrl("/");


            Task anchorParser = Task.Factory.StartNew(() => Run(baseUrl, queue, console));
            anchorParser.Wait();
        }

        private static void Run(string baseUrl, CrawlQueue queue, Console console)
        {
            var snatcher = new Snatcher(baseUrl, console);

            Url url;
            while ((url = queue.GetNext()) != null)
            {
                Page page = snatcher.GetPage(url);

                string[] anchors = page.FindLocalPages();
                foreach (string anchor in anchors)
                {
                    queue.AddUrl(anchor);
                }
            }
        }
    }
}
