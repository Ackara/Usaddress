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
			FirstName = firstName;
			MiddleName = middleName;
			LastName = lastName;
			Suffix = suffix;
		}

		/// <summary>
		/// Gets or sets the person's first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the middle name.
		/// </summary>
		public string MiddleName { get; set; }

		/// <summary>
		/// Gets or sets the person's last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the suffix. (e.g. PhD, Jr, etc)
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
			return string.Join(" ", FirstName, MiddleName, LastName, Suffix).Replace("  ", " ").Trim();
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

		/// <summary>
		/// Determines if the specified names are equal.
		/// </summary>
		/// <param name="a">The first name.</param>
		/// <param name="b">The other name.</param>
		/// <param name="comparison">The comparison (defaults to <see cref="StringComparison.InvariantCultureIgnoreCase"/>).</param>
		public static bool Equals(FullName a, FullName b, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return
				string.Equals(a.FirstName, b.FirstName, comparison)
				&&
				string.Equals(a.MiddleName, b.MiddleName, comparison)
				&&
				string.Equals(a.LastName, b.LastName, comparison)
				&&
				string.Equals(a.Suffix, b.Suffix, comparison)
				;
		}

		public static bool operator ==(FullName x, FullName y) => FullName.Equals(x, y);

		public static bool operator !=(FullName x, FullName y) => !FullName.Equals(x, y);

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
		/// </returns>
		public bool Equals(FullName other) => Equals(this, other);

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj) => (obj is FullName ? Equals(this, (FullName)obj) : false);

		public override int GetHashCode()
		{
			return
				FirstName?.GetHashCode() ?? 0 ^
				MiddleName?.GetHashCode() ?? 0 ^
				LastName?.GetHashCode() ?? 0 ^
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
