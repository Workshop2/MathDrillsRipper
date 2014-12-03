using System;
using System.Collections.Generic;
using System.Linq;

namespace MathDrillsRipper
{
    public class CrawlQueue
    {
        private readonly IConsole _console;
        private readonly object _lock = new object();
        private readonly Queue<string> _queuedUrls = new Queue<string>();
        private readonly List<string> _crawledUrls = new List<string>();
        public int TotalQueued { get { lock (_lock) { return _queuedUrls.Count; } } }
        public int TotalCrawled { get { lock (_lock) { return _crawledUrls.Count; } } }

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

            if (IsNewUrl(originalUrl))
            {
                _console.WriteInfo("Queuing url: {0}", originalUrl);

                lock (_lock)
                {
                    _queuedUrls.Enqueue(originalUrl);
                }
            }
            else
            {
                _console.WriteWarning("Skipping {0}", originalUrl);
            }
        }

        public string GetNext()
        {
            string next = null;
            lock (_lock)
            {
                if (_queuedUrls.Any())
                {
                    next = _queuedUrls.Dequeue();
                    _crawledUrls.Add(next);
                }
            }
            return next;
        }

        private bool IsNewUrl(string url)
        {
            //return !(_queuedUrls.Contains(url) && _crawledUrls.Contains(url));

            bool isNew = true;

            lock (_lock)
            {
                if (_queuedUrls.Any(x => x.Equals(url, StringComparison.InvariantCultureIgnoreCase)))
                {
                    isNew = false;
                }
                else if (_crawledUrls.Any(x => x.Equals(url, StringComparison.InvariantCultureIgnoreCase)))
                {
                    isNew = false;
                }
            }

            return isNew;
        }
    }
}