using System.Collections.Generic;

namespace MathDrillsRipper
{
    public class CrawlQueue
    {
        private readonly IConsole _console;
        private readonly object _lock = new object();
        private readonly Queue<Url> _queuedUrls = new Queue<Url>();
        private readonly List<Url> _crawledUrls = new List<Url>();

        public CrawlQueue(IConsole console)
        {
            _console = console;
        }

        public void AddUrl(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                _console.WriteError("Error: No url detected to add.");
                return;
            }
            
            var url = new Url { OriginalUrl = originalUrl };
            if (IsNewUrl(url))
            {
                _console.WriteInfo("Queuing url: {0}", originalUrl);

                lock (_lock)
                {
                    _queuedUrls.Enqueue(url);
                }
            }
        }

        public Url GetNext()
        {
            lock (_lock)
            {
                Url next = _queuedUrls.Dequeue();
                _crawledUrls.Add(next);
                return next;
            }
        }

        private bool IsNewUrl(Url url)
        {
            lock (_lock)
            {
                return !(_queuedUrls.Contains(url) || _crawledUrls.Contains(url));
            }
        }
    }
}