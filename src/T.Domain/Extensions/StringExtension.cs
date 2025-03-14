using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace T.Domain.Extensions {
    public static class StringExtension {
        private const string SpecialCharsPattern = @"[_*\[\]()\~`>#\+\-=|{}.!]";

        [return: NotNullIfNotNull(nameof(text))]
        public static string? UnsignedUnicode(this string? text) {
            if (string.IsNullOrWhiteSpace(text)) return text;
            var chars = text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC).Trim().ToLower().Replace('đ', 'd');
        }

        public static string UnsignedUnicode(this string? text, string defaultValue) {
            return UnsignedUnicode(text) ?? defaultValue;
        }

        public static string GetLast(this string text, int length = 4) {
            if (string.IsNullOrWhiteSpace(text) || length >= text.Length)
                return text;
            return text[^length..];
        }

        [return: NotNullIfNotNull(nameof(text))]
        public static string? ReplaceSpace(string? text, bool isUnsignedUnicode = false) {
            if (text == null) return text;
            text = Regex.Replace(text.Trim(), @"\s+", " ");

            if (isUnsignedUnicode) {
                text = UnsignedUnicode(text);
            }

            return text;
        }

        [return: NotNullIfNotNull(nameof(text))]
        public static string? ReplaceSpaceToHyphen(string? text) {
            if (text == null) return text;
            text = text.Trim();

            return text.Replace(" ", "-");
        }

        public static string GeneratePassword(int length = 8) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string ReplaceTelegramSpecialChars(string text) {
            if (string.IsNullOrEmpty(text)) return text;
            return Regex.Replace(text, SpecialCharsPattern, "\\$0");
        }
    }
}
