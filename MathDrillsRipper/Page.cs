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

        public string[] FindLocalPages()
        {
            return FindLocalAnchors().Where(x => !IsPdf(x)).ToArray();
        }

        public string[] FindPdfs()
        {
            return FindLocalAnchors().Where(IsPdf).ToArray();
        }

        private IEnumerable<string> FindLocalAnchors()
        {
            CQ anchors = _document["a"];
            IEnumerable<string> localAnchors = anchors
                                                    .Where(x => x.HasAttribute("href"))
                                                    .Select(x => x.GetAttribute("href"))
                                                    .Where(x => x.StartsWith("/"));

            localAnchors = StripHashTags(localAnchors);
            return localAnchors;
        }

        private bool IsPdf(string anchor)
        {
            return anchor.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase);
        }

        private IEnumerable<string> StripHashTags(IEnumerable<string> localAnchors)
        {
            return localAnchors
                        .Select(x => x.Contains("#") ? x.Substring(0, x.IndexOf("#", System.StringComparison.Ordinal)) : x)
                        .Distinct();
        }
    }
}