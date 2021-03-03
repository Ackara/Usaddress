using System;

namespace Tekcari.MailN
{
	internal class AddressFormatter : ICustomFormatter, IFormatProvider
	{
		public object GetFormat(Type formatType)
		{
			return formatType == typeof(ICustomFormatter) ? this : null;
		}

		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if (arg is Address address)
			{
				char c;
				int n = format.Length;
				var builder = new System.Text.StringBuilder();

				for (int i = 0; i < n; i++)
					switch (c = format[i])
					{
						default: builder.Append(c); break;

						case 'G': builder.Append(address.ToString()); break;

						case 'C':
							builder.AppendLine($"{address.Street}")
								   .AppendLine($"{address.City}, {address.State} {address.PostalCode}")
								   .Append(address.Country);
							break;

						case 'c':
							builder.Append($"{address.Street} ")
								   .Append($"{address.City}, {address.State} {address.PostalCode} ")
								   .Append(address.Country);
							break;

						case '0':
							builder.Append(address.Street); break;

						case '1':
							builder.Append(address.Street1); break;

						case '2':
							builder.Append(address.Street2); break;

						case '3':
							builder.Append(address.City); break;

						case '4':
							builder.Append(address.State); break;

						case '5':
							builder.Append(address.Country); break;

						case '6':
							builder.Append(address.PostalCode); break;

						case '\\': /* Escape */
							if ((i + 1) < n)
								builder.Append(format[++i]);
							else
								builder.Append(c);
							break;
					}

				return builder.ToString().Trim();
			}
			else return GetFallbackFormat(format, arg);
		}

		private string GetFallbackFormat(string format, object arg)
		{
			if (arg is IFormattable)
				return ((IFormattable)arg).ToString(format, System.Globalization.CultureInfo.CurrentCulture);
			else if (arg != null)
				return arg.ToString();
			else
				return string.Empty;
		}
	}
}
