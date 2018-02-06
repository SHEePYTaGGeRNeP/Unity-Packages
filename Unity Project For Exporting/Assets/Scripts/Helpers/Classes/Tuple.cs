using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Helpers.Classes
{
    public static class Tuple
    {
        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>(item1, item2);
        }
    }

    [DebuggerDisplay("Item1={Item1};Item2={Item2}")]
    [Serializable]
    public class Tuple<T1, T2> : IFormattable
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public Tuple(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        #region Optional - If you need to use in dictionaries or check equality
        private static readonly IEqualityComparer<T1> Item1Comparer = EqualityComparer<T1>.Default;
        private static readonly IEqualityComparer<T2> Item2Comparer = EqualityComparer<T2>.Default;

        public override int GetHashCode()
        {
            var hc = 0;
            if (!ReferenceEquals(this.Item1, null))
                hc = Item1Comparer.GetHashCode(this.Item1);
            if (!ReferenceEquals(this.Item2, null))
                hc = (hc << 3) ^ Item2Comparer.GetHashCode(this.Item2);
            return hc;
        }
        public override bool Equals(object obj)
        {
            var other = obj as Tuple<T1, T2>;
            if (ReferenceEquals(other, null))
                return false;
            else
                return Item1Comparer.Equals(this.Item1, other.Item1) && Item2Comparer.Equals(this.Item2, other.Item2);
        }
        #endregion

        #region Optional - If you need to do string-based formatting
        public override string ToString() { return this.ToString(null, CultureInfo.CurrentCulture); }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, format ?? "{0},{1}", this.Item1, this.Item2);
        }
        #endregion
    }
}