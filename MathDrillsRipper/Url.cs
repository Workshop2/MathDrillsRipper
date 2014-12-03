using System;
using System.Diagnostics;

namespace MathDrillsRipper
{
    [DebuggerDisplay("OriginalUrl = {OriginalUrl}")]
    public class Url
    {
        public string OriginalUrl { get; set; }

        protected bool Equals(Url other)
        {
            bool equals = OriginalUrl.Equals(other.OriginalUrl, StringComparison.InvariantCultureIgnoreCase);
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Url) obj);
        }
    }
}