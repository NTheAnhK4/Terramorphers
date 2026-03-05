using GameCore.Utility.Common;

namespace GameCore.Utility
{
    public static class NumberExtensions
    {
        private static readonly NumberShortenerFormatter Formatter = new ();
        
        public static string ShortenSuffix(this int number)
        {
            return string.Format(Formatter, "{0}", number);
        }
    }
}