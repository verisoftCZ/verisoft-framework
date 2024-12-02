using System.Globalization;
using System.Text;

namespace Verisoft.Core.Common
{
    public static class StringExtensions
    {
        public static string ReplaceAccents(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                var c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        public static string SubstringAndTrimToLastWord(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.Length > maxLength)
            {
                value = value.Substring(0, maxLength);
                var pos = value.LastIndexOf(' ');
                if (pos != -1)
                {
                    value = value.Substring(0, pos);
                }
            }

            return value;
        }
    }
}
