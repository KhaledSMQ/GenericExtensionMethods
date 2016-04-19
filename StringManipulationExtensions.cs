using System;
using System.Text;
using System.Text.RegularExpressions;

namespace GenericExtensionMethods
{
    // Payton Byrd - 2010/01/19
    // All commented code will be removed for version 1.0.0

    /// <summary>
    /// Provides useful string functions.
    /// </summary>
    public static class StringManipulationExtensions
    {
        /// <summary>
        /// Formats the string with the supplied data.
        /// </summary>
        /// <param name="source">The format of the string to return.</param>
        /// <param name="data">The data to apply to the format.</param>
        /// <returns>The formatted string.</returns>
        /// <exception cref="System.ArgumentNullException" />
		public static string FormatString(
			this string source,
			params object[] data)
		{
			source.AssertParameterNotNull(
				"Cannot use null string as a format expression.",
				"source");

			return string.Format(source, data);
		}

        /// <summary>
        /// Appends the supplied format to
        /// the end of the source string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="format">The format to apply.</param>
        /// <param name="data">The data to format.</param>
        /// <returns>The source string with the formatted
        /// string appended at the end.</returns>
        /// <exception cref="System.ArgumentNullException" />
        public static string AppendFormat(
            this string source,
            string format,
            params object[] data)
        {
            format.AssertParameterNotNull(
                "Cannot perform formatting of null string.",
                "format");

            format.AssertParameterNotEmpty(
                "Cannot perform formatting of empty string.",
                "format");

            return (source = 
                (source ?? string.Empty) 
                + format.FormatString(data));
        }

        /// <summary>
        /// Determines if the source string is
        /// contained in the array of strings provided.
        /// </summary>
        /// <param name="source">The string to compare.</param>
        /// <param name="strings">The list of strings to 
        /// compare against.</param>
        /// <returns>True if the source string is found in the 
        /// array of test strings.</returns>
        /// <exception cref="System.ArgumentNullException" />
        public static bool IsIn(
            this string source,
            string[] strings)
        {
            strings.AssertParameterNotNull(
                "The string array cannot be null.",
                "strings");

            foreach (string current in strings)
            {
                if (current.Equals(source))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Writes the source string to
        /// the console as a line.
        /// </summary>
        /// <param name="source">The string
        /// to write to the Console.</param>
        /// <returns>The source string.</returns>
        /// <remarks>
        /// This method can be chained with
        /// other string extensions such as
        /// writing to both the console and
        /// debug outputs.
        /// <example>
        /// <code>
        /// "Test {0}".Format("Data").ToConsole().ToDebug();
        /// </code>
        /// This would write "Test Data" to both the
        /// console and debug outputs.</example>
        /// </remarks>
        public static string ToConsole(
            this string source)
        {
            Console.WriteLine(source);
            return source;
        }

        /// <summary>
        /// Writes the formatted string
        /// to the console.
        /// </summary>
        /// <param name="source">The format to return.</param>
        /// <param name="data">The data to fill the format.</param>
        /// <returns>The formatted string.</returns>
        /// <remarks>
        /// This method can be chained with
        /// other string extensions such as
        /// writing to both the console and
        /// debug outputs.
        /// <example>
        /// <code>
        /// "Test {0}".ToConsole("Data").ToDebug();
        /// </code>
        /// This would write "Test Data" to both the
        /// console and debug outputs.</example>
        /// </remarks>
        public static string ToConsole(
            this string source,
            params object[] data)
        {
            string result = source.FormatString(data);
            Console.WriteLine(result);
            return result;
        }

        /// <summary>
        /// Writes the source string to
        /// the debug output as a line.
        /// </summary>
        /// <param name="source">The string
        /// to write to the Console.</param>
        /// <returns>The source string.</returns>
        /// <remarks>
        /// This method can be chained with
        /// other string extensions such as
        /// writing to both the console and
        /// debug outputs.
        /// <example>
        /// <code>
        /// "Test {0}".Format("Data").ToConsole().ToDebug();
        /// </code>
        /// This would write "Test Data" to both the
        /// console and debug outputs.</example>
        /// </remarks>
        public static string ToDebug(
            this string source)
        {
            System.Diagnostics.Debug.WriteLine(source);
            return source;
        }

        /// <summary>
        /// Writes the formatted string
        /// to the debug output.
        /// </summary>
        /// <param name="source">The format to return.</param>
        /// <param name="data">The data to fill the format.</param>
        /// <returns>The formatted string.</returns>
        /// <remarks>
        /// This method can be chained with
        /// other string extensions such as
        /// writing to both the console and
        /// debug outputs.
        /// <example>
        /// <code>
        /// "Test {0}".ToDebug("Data").ToConsole();
        /// </code>
        /// This would write "Test Data" to both the
        /// console and debug outputs.</example>
        /// </remarks>
        public static string ToDebug(
            this string source,
            params object[] data)
        {
            string result = source.FormatString(data);
            System.Diagnostics.Debug.WriteLine(result);
            return result;
        }

        /// <summary>
        /// Repeats the source character
        /// to debug the specified number of times.
        /// </summary>
        /// <param name="c">Character to repeat.</param>
        /// <param name="repeat">Number of times to repeat the character.</param>
		public static void ToDebug(
			this char c, 
			int repeat)
		{
			new string(c, repeat).ToDebug();
		}

        /// <summary>
        /// Returns a 
        /// <see cref="System.Text.StringBuilder"/>
        /// pre-populated with the source string.
        /// </summary>
        /// <param name="source">The string
        /// to pre-populate the StringBuilder.</param>
        /// <returns>A new StringBuilder containing
        /// the supplied string.</returns>
        /// <remarks>
        /// If the source is null, an empty string 
        /// is used to initialize the StringBuilder.
        /// </remarks>
        public static StringBuilder AsStringBuilder(
            this string source)
        {
            return new StringBuilder(source ?? string.Empty);
        }

        /// <summary>
        /// Performs a 
        /// <see cref="System.Text.RegularExpressions.Regex"/>
        /// match against the source string
        /// using the supplied pattern that
        /// returns all matches.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns>A collection of matches.</returns>
        public static MatchCollection Matches(
            this string source,
            string pattern)
        {
            return new Regex(pattern).Matches(source);
        }

        /// <summary>
        /// Performs a 
        /// <see cref="System.Text.RegularExpressions.Regex"/>
        /// match against the source string
        /// using the supplied pattern that
        /// returns the first match.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns>The first matching substring.</returns>
        public static Match Match(
            this string source,
            string pattern)
        {
            return new Regex(pattern).Match(source);
        }

        /// <summary>
        /// Returns an ASCII byte array
        /// of the source string.
        /// </summary>
        /// <param name="source">The string to convert
        /// to a byte array.</param>
        /// <returns>The ASCII encoded byte array.</returns>
        public static byte[] ToAsciiByteArray(
            this string source)
        {
            byte[] buffer = null;

            ASCIIEncoding encoding = new ASCIIEncoding();
            buffer = encoding.GetBytes(source.ToCharArray());

            return buffer;
        }

        /// <summary>
        /// Returns a byte array based
        /// upon the supplied
        /// <see cref="System.Text.Encoding"/>.
        /// </summary>
        /// <typeparam name="E">The type of
        /// Encoding to use for the conversion.</typeparam>
        /// <param name="source">The string to convert
        /// to a byte array.</param>
        /// <param name="encoding">Instance of the
        /// encoding objec to use.</param>
        /// <returns>The byte array based upon
        /// the supplied encoding.</returns>
        /// <exception cref="System.ArgumentNullException" />
        public static byte[] ToByteArray<E>(
            this string source)
            where E : System.Text.Encoding, new()
        {
            byte[] buffer = null;
            E encoding = new E();

            buffer = encoding.GetBytes(source.ToCharArray());

            return buffer;
        }

		/// <summary>
		/// Returns null if the string is either
		/// null or empty.
		/// </summary>
		/// <param name="source">String to test.</param>
		/// <returns>Value of the string, or null.</returns>
		public static string NullIfEmpty(
			this string source)
		{
			return (source.IsNullOrEmpty() ? null : source);
		}

        /// <summary>
        /// Converts the string to an enum value.
        /// </summary>
        /// <typeparam name="E">Type of the enum to return.</typeparam>
        /// <param name="source">String to convert.</param>
        /// <returns>Enum value for the specified string.</returns>
		public static E ParseEnum<E>(
			this string source)
		{
			E result = (E)Enum.Parse(typeof(E), source);

			return result;
		}


        public static bool EqualsIgnoreCase(
            this string source,
            string target)
        {
            if ((source.IsNull() && target.IsNotNull()) || (source.IsNotNull() && target.IsNull())) return false;
            return source.ToLower() == target.ToLower();
        }

    }
}
