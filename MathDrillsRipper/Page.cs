using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;

namespace MathDrillsRipper
{
    public class Page
    {
        private readonly CQ _document;

        public Page(CQ document)
        {
            _document = document;
        }

        public IEnumerable<string> FindLocalPages(string baseUrl)
        {
            return FindLocalAnchors(baseUrl).Where(x => !IsPdf(x));
        }

        public IEnumerable<string> FindPdfs(string baseUrl)
        {
            CQ anchors = _document["a"];
            return anchors
                .Where(x => x.HasAttribute("href"))
                .Select(x => x.GetAttribute("href"))
                .Where(IsPdf);
        }

        private IEnumerable<string> FindLocalAnchors(string baseUrl)
        {
            CQ anchors = _document["a"];
            baseUrl = baseUrl.EndsWith("/") ? baseUrl.Substring(0, baseUrl.Length - 1) : baseUrl;

            IEnumerable<string> localAnchors = anchors
                .Where(x => x.HasAttribute("href"))
                .Select(x => x.GetAttribute("href"))
                .Where(x => x.StartsWith("/") || (x.StartsWith(baseUrl, StringComparison.InvariantCultureIgnoreCase) && !x.Equals(baseUrl, StringComparison.InvariantCultureIgnoreCase)))
                .Select(x => x.StartsWith(baseUrl, StringComparison.InvariantCultureIgnoreCase) ? x.Substring(baseUrl.Length) : x)
                .Select(x => x.Contains("#") ? x.Substring(0, x.IndexOf("#", StringComparison.Ordinal)) : x)
                .Select(x => x.StartsWith("/") ? x.Substring(1) : x)
                .Select(x => x.EndsWith("/") ? x.Substring(0, x.Length - 1) : x)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct();

            return localAnchors;
        }

        private bool IsPdf(string anchor)
        {
            return anchor.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase);
        }

    }
}