using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MarcoWillems.Template.BasicMicroservice.Services.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNotNullOrEmpty(this string? s)
        {
            return !s.IsNullOrEmpty();
        }

        public static string EscapeFileName(this string s)
        {
            return Regex.Replace(s, "[^a-zA-Z0-9]", "-");
        }

        public static string RemoveDiacritics(this string s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < normalizedString.Length; i++)
            {
                var c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        public static string Capitalize(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            var charArray = s.ToCharArray();
            charArray[0] = char.ToUpper(charArray[0], CultureInfo.InvariantCulture);

            return new string(charArray);
        }

        public static string UrlEncode(this string s)
        {
            return WebUtility.UrlEncode(s);
        }

        public static string AddUrlParameters(
            this string s,
            params (string name, string value)[] parameters)
        {
            var dictionary = parameters.ToDictionary(
                p => p.name,
                p => p.value);

            return s.AddUrlParameters(dictionary);
        }

        public static string AddUrlParameters(
            this string s,
            IDictionary<string, string>? parameters)
        {
            var result = parameters != null
                ? $"{s}?{string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"))}"
                : s;

            return result;
        }
    }
}
