using System;

namespace Tekcari.Usaddress
{
	public struct Address : IEquatable<Address>, IFormattable
	{
		public Address(string street1 = default, string street2 = default, string city = default, string state = default, string postalCode = default, string country = default)
		{
			Line1 = street1;
			Line2 = street2;
			City = city;
			State = state;
			Country = country;
			PostalCode = postalCode;
		}

		/// <summary>
		/// Get or set the street address.
		/// </summary>
		public string Line1 { get; set; }

		/// <summary>
		/// Get or set the street address.
		/// </summary>
		public string Line2 { get; set; }

		/// <summary>
		/// Get the <see cref="Line1"/> and <see cref="Line2"/> members concatenated.
		/// </summary>
		public string Street
		{
			get => string.Concat(Line1, ' ', Line2).Trim();
		}

		/// <summary>
		/// Get or set the city.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Get or set the state.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Get or set the country
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Get or set the postal code.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Create a new <see cref="Address"/> from text.
		/// </summary>
		/// <param name="text">The text.</param>
		public static Address Parse(string text)
		{
			if (string.IsNullOrEmpty(text)) return new Address();

			string[] valuePair;
			string[] parts = text.Split(';', '\0');
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
						result.Line1 = valuePair[1];
						break;

					case "S2":
					case "ST2":
					case "STREET2":
						result.Line2 = valuePair[1];
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

		/// <summary>Converts to string.</summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Concat(
				"STREET1=", Line1, ';',
				"STREET2=", Line2, ';',
				"CITY=", City, ';',
				"STATE=", State, ';',
				"COUNTRY=", Country, ';',
				"ZIP=", PostalCode
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

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="x">The first address.</param>
		/// <param name="y">The second address.</param>
		/// <returns></returns>
		public static bool Equals(Address x, Address y)
		{
			return
				string.Equals(x.Line1, y.Line1, StringComparison.InvariantCultureIgnoreCase)
				&&
				string.Equals(x.Line2, y.Line2, StringComparison.InvariantCultureIgnoreCase)
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

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
		/// </returns>
		public bool Equals(Address other) => Equals(this, other);

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj) => (obj is Address ? Equals(this, (Address)obj) : false);

		public static bool operator ==(Address x, Address y) => Address.Equals(x, y);

		public static bool operator !=(Address x, Address y) => !Address.Equals(x, y);

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return
				(Line1?.ToUpperInvariant()?.GetHashCode() ?? 0) ^
				(Line2?.ToUpperInvariant()?.GetHashCode() ?? 0) ^
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
