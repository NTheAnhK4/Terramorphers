using System;
using System.Collections.Generic;

namespace GameCore.Utility.Common
{
    public class NumberShortenerFormatter : ICustomFormatter, IFormatProvider
    {
        private static readonly List<(int, int, string)> Abbreviations = new()
        {
            (1000000000, 1000000000, "b"),
            (1000000, 1000000, "m"),
            (10000, 1000, "k"),
        };

        public object GetFormat(Type formatType)
        {
            return (formatType == typeof(ICustomFormatter)) ? this : null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Convert the numeric value to double
            var doubleValue = Convert.ToDouble(arg);

            // Iterate over abbreviations in descending order
            foreach (var (threshold, divisor, abbreviation) in Abbreviations)
            {
                // Check if the absolute value of the double value is greater than or equal to the threshold
                if (!(Math.Abs(doubleValue) >= threshold)) continue;

                // Calculate the rounded number and format it with the abbreviation
                var roundedNumber = doubleValue / divisor;
                return $"{roundedNumber:0.#}{abbreviation}";
            }

            // If no abbreviation is applied, return the original double value formatted
            return $"{doubleValue:0.#}";
        }
    }
}