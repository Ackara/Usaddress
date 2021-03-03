using System;

namespace Tekcari.MailN
{
	/// <summary>
	/// Represents a person's name.
	/// </summary>
	/// <seealso cref="System.IEquatable{Tekcari.MailN.FullName}" />
	/// <seealso cref="System.IFormattable" />
	public struct FullName : IEquatable<FullName>, IFormattable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FullName" /> struct.
		/// </summary>
		/// <param name="firstName">The given name.</param>
		/// <param name="lastName">The family name.</param>
		/// <param name="middleName">The middle name.</param>
		/// <param name="suffix">The suffix.</param>
		public FullName(string firstName, string lastName = default, string middleName = default, string suffix = default)
		{
			Given = firstName;
			Middle = middleName;
			Family = lastName;
			Suffix = suffix;
		}

		/// <summary>
		/// Gets or sets the given name.
		/// </summary>
		public string Given { get; set; }

		/// <summary>
		/// Gets or sets the middle name.
		/// </summary>
		public string Middle { get; set; }

		/// <summary>
		/// Gets or sets the family name.
		/// </summary>
		public string Family { get; set; }

		/// <summary>
		/// Gets or sets the suffix.
		/// </summary>
		public string Suffix { get; set; }

		/// <summary>
		/// Parses the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		public static FullName Parse(string text)
		{
			if (string.IsNullOrEmpty(text)) return new FullName();

			string[] parts = text.Split(' ');
			string get(int idx) => idx >= parts.Length ? null : parts[idx];

			switch (parts.Length)
			{
				case 1:
				case 2:
					return new FullName(get(0), get(1));

				case 3:
					string last = get(2);

					return (IsSuffix(last) ?
						new FullName(get(0), get(1), default, last)
						:
						new FullName(get(0), get(2), get(1)));

				case 4:
					return new FullName(get(0), get(2), get(1), get(3));
			}

			return new FullName();
		}

		#region IFormattable

		/// <summary>Converts to string.</summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Join(" ", Given, Middle, Family, Suffix).Replace("  ", " ").Trim();
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
			return ToString(string.Concat("{0:", (string.IsNullOrEmpty(format) ? "G" : format), '}'), Formatter);
		}

		/// <summary>
		/// Converts to string.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format(
				formatProvider ?? Formatter,
				(string.IsNullOrEmpty(format) ? "{0:G}" : format),
				this);
		}

		public static implicit operator string(FullName obj) => obj.ToString();

		#endregion IFormattable

		#region IEquatable

		public static bool Equals(FullName a, FullName b)
		{
			return
				string.Equals(a.Given, b.Given, StringComparison.InvariantCultureIgnoreCase)
				&&
				string.Equals(a.Middle, b.Middle, StringComparison.InvariantCultureIgnoreCase)
				&&
				string.Equals(a.Family, b.Family, StringComparison.InvariantCultureIgnoreCase)
				&&
				string.Equals(a.Suffix, b.Suffix, StringComparison.InvariantCultureIgnoreCase)
				;
		}

		public static bool operator ==(FullName x, FullName y) => FullName.Equals(x, y);

		public static bool operator !=(FullName x, FullName y) => !FullName.Equals(x, y);

		public bool Equals(FullName other) => Equals(this, other);

		public override bool Equals(object obj) => (obj is FullName ? Equals(this, (FullName)obj) : false);

		public override int GetHashCode()
		{
			return
				Given?.GetHashCode() ?? 0 ^
				Middle?.GetHashCode() ?? 0 ^
				Family?.GetHashCode() ?? 0 ^
				Suffix?.GetHashCode() ?? 0;
		}

		#endregion IEquatable

		#region Backing Members

		internal static bool IsSuffix(string text)
		{
			/// NOTE: A suffix is a string that ends with a period (.)
			/// or has a uppercase beyond the first character.

			if (string.IsNullOrEmpty(text)) return false;
			if (text[text.Length - 1] == '.') return true;

			char c;
			for (int i = 1; i < text.Length; i++)
			{
				c = text[i];
				if (char.IsLetter(c) && char.IsUpper(c)) return true;
			}

			return ((text.Length == 3 || text.Length == 4) || IsAllCaps(text));
		}

		internal static bool IsAllCaps(string text)
		{
			char c;
			for (int i = 0; i < text.Length; i++)
			{
				c = text[i];
				if (char.IsLetter(c) && char.IsLower(c)) return false;
			}

			return true;
		}

		private static NameFormatter _formatter;

		private NameFormatter Formatter
		{
			get
			{
				if (_formatter == null) _formatter = new NameFormatter();
				return _formatter;
			}
		}

		#endregion Backing Members
	}
}
