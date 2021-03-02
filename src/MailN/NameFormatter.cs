using System;

namespace Acklann.MailN
{
    internal class NameFormatter : ICustomFormatter, IFormatProvider
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is FullName name)
            {
                char c;
                int n = format.Length;
                var builder = new System.Text.StringBuilder();

                for (int i = 0; i < n; i++)
                    switch (c = format[i])
                    {
                        default: builder.Append(c); break;

                        case 'G': builder.Append(name.ToString()); break;

                        case '1':
                        case 'f':
                        case 'F':
                            builder.Append(name.Given); break;

                        case '2':
                        case 'm':
                        case 'M':
                            builder.Append(name.Middle); break;

                        case '3':
                        case 'l':
                        case 'L':
                            builder.Append(name.Family); break;

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