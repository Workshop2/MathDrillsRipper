using System;

namespace MathDrillsRipper
{
    public class Url
    {
        public string OriginalUrl { get; set; }

        protected bool Equals(Url other)
        {
            return OriginalUrl.Equals(other.OriginalUrl, StringComparison.InvariantCultureIgnoreCase);
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