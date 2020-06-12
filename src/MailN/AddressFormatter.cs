using System;

namespace Acklann.MailN
{
    public class AddressFormatter : ICustomFormatter, IFormatProvider
    {
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

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
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