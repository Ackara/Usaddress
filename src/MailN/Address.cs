using System;
using System.Runtime.Serialization;

namespace Acklann.MailN
{
    public struct Address : IEquatable<Address>, IFormattable
    {
        public Address(string street1 = default, string street2 = default, string city = default, string state = default, string country = default, string postalCode = default)
        {
            Street1 = street1;
            Street2 = street2;
            City = city;
            State = state;
            Country = country;
            PostalCode = postalCode;
        }

        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string Street
        {
            get => string.Concat(Street1, ' ', Street2).Trim();
        }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public static Address Parse(string text)
        {
            if (string.IsNullOrEmpty(text)) return new Address();

            string[] valuePair;
            string[] parts = text.Split(';');
            var result = new Address();

            for (int i = 0; i < parts.Length; i++)
            {
                valuePair = parts[i].Split('=');
                if (valuePair.Length != 2) continue;

                switch (valuePair[0].ToUpperInvariant())
                {
                    case "S1":
                    case "ST":
                    case "ST1":
                    case "STREET1":
                        result.Street1 = valuePair[1];
                        break;

                    case "S2":
                    case "ST2":
                    case "STREET2":
                        result.Street2 = valuePair[1];
                        break;

                    case "C":
                    case "CTY":
                    case "CITY":
                        result.City = valuePair[1];
                        break;

                    case "S":
                    case "STATE":
                        result.State = valuePair[1];
                        break;

                    case "CO":
                    case "CTRY":
                    case "COUNTRY":
                        result.Country = valuePair[1];
                        break;

                    case "Z":
                    case "ZIP":
                    case "POSTALCODE":
                        result.PostalCode = valuePair[1];
                        break;
                }
            }

            return result;
        }

        #region IFormattable

        public override string ToString()
        {
            return string.Concat(
                "STREET1=", Street1, ';',
                "STREET2=", Street2, ';',
                "CITY=", City, ';',
                "STATE=", State, ';',
                "COUNTRY=", Country, ';',
                "ZIP=", PostalCode
                );
        }

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

        public static bool Equals(Address x, Address y)
        {
            return
                string.Equals(x.Street1, y.Street1, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(x.Street2, y.Street2, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(x.City, y.City, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(x.State, y.State, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(x.Country, y.Country, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(x.PostalCode, y.PostalCode, StringComparison.InvariantCultureIgnoreCase)
                ;
        }

        public static bool operator ==(Address x, Address y) => Address.Equals(x, y);

        public static bool operator !=(Address x, Address y) => !Address.Equals(x, y);

        public bool Equals(Address other) => Equals(this, other);

        public override bool Equals(object obj) => (obj is Address ? Equals(this, (Address)obj) : false);

        public override int GetHashCode()
        {
            return
                (Street1?.ToUpperInvariant()?.GetHashCode() ?? 0) ^
                (Street2?.ToUpperInvariant()?.GetHashCode() ?? 0) ^
                (City?.ToUpperInvariant()?.GetHashCode() ?? 0) ^
                (State?.ToUpperInvariant()?.GetHashCode() ?? 0) ^
                (Country?.ToUpperInvariant()?.GetHashCode() ?? 0) ^
                (PostalCode?.ToUpperInvariant()?.GetHashCode() ?? 0)
                ;
        }

        #endregion IEquatable

        #region Operators

        public static explicit operator Address(string text) => Parse(text);

        public static implicit operator string(Address address) => address.ToString();

        #endregion Operators
    }
}