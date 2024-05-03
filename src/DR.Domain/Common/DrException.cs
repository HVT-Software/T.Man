using System.Diagnostics.CodeAnalysis;

namespace DR.Domain.Common {

    public sealed class DrException(string message) : Exception(message) {

        public static void ThrowIf([DoesNotReturnIf(true)] bool when, string message) {
            if (when) Throw(message);
        }

        public static void ThrowIfFalse([DoesNotReturnIf(false)] bool when, string message) {
            if (!when) Throw(message);
        }

        public static void ThrowIfNull([NotNull] object? obj, string message) {
            if (obj is null) Throw(message);
        }

        public static T ThrowIfNull<T>([NotNull] T? obj, string message) {
            if (obj is null) Throw(message);
            return obj;
        }

        public static void ThrowIfNullOrEmpty<T>([NotNull] List<T>? list, string message) {
            ThrowIf(list == null || list.Count == 0, message);
        }

        public static void ThrowIfNullOrEmpty([NotNull] string? value, string message) {
            ThrowIf(string.IsNullOrWhiteSpace(value), message);
        }

        [DoesNotReturn]
        public static void Throw(string message) {
            throw new DrException(message);
        }
    }
}
