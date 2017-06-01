namespace ConsoleApp.Akka
{
    internal static class StringExtensions
    {
        public static string NullIfEmpty(this string self) => string.IsNullOrEmpty(self) ? null : self;
    }
}