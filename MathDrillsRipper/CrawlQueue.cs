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
        private readonly HashSet<string> _crawledUrls = new HashSet<string>();
        public int TotalQueued { get { lock (_lock) { return _queuedUrls.Count; } } }
        public int TotalCrawled { get { lock (_lock) { return _crawledUrls.Count; } } }

        public CrawlQueue(IConsole console)
        {
            _console = console;
        }

        public void AddUrl(IEnumerable<string> urls)
        {
            lock (_lock)
            {
                foreach (string url in urls)
                {
                    if (string.IsNullOrEmpty(url))
                    {
                        _console.WriteError("Error: No url detected to add.");
                        return;
                    }

                    if (IsNewUrl(url))
                    {
                        _console.WriteInfo("Queuing url: {0}", url);

                        _queuedUrls.Enqueue(url);
                    }
                    else
                    {
                        _console.WriteWarning("Skipping {0}", url);
                    }
                }
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
                    _crawledUrls.Add(next.ToLowerInvariant());
                }
            }
            return next;
        }

        private bool IsNewUrl(string url)
        {
            bool isNew = true;
            if (_queuedUrls.Any(x => x.Equals(url, StringComparison.InvariantCultureIgnoreCase)))
            {
                isNew = false;
            }
            else if (_crawledUrls.Contains(url.ToLowerInvariant()))
            {
                isNew = false;
            }

            return isNew;
        }
    }
}