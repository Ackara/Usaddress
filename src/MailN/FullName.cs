using System;

namespace Acklann.MailN
{
    public struct FullName : IEquatable<FullName>, IFormattable
    {
        public FullName(string first, string middle, string last)
        {
            Given = first;
            Middle = middle;
            Family = last;
        }

        public string Given { get; set; }

        public string Middle { get; set; }

        public string Family { get; set; }

        #region IFormattable

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Concat(
                "FIRST=", Given, ';',
                "MIDDLE=", Middle, ';',
                "SUR=", Family
                );
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return ToString(string.Concat("{0:", (string.IsNullOrEmpty(format) ? "G" : format), '}'), new AddressFormatter());
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format(
                formatProvider ?? new AddressFormatter(),
                (string.IsNullOrEmpty(format) ? "{0:G}" : format),
                this);
        }

        #endregion IFormattable

        #region IEquatable

        public static bool Equals(FullName x, FullName y)
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(FullName x, FullName y) => FullName.Equals(x, y);

        public static bool operator !=(FullName x, FullName y) => !FullName.Equals(x, y);

        public bool Equals(FullName other) => Equals(this, other);

        public override bool Equals(object obj) => (obj is FullName ? Equals(this, (FullName)obj) : false);

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        #endregion IEquatable
    }
}