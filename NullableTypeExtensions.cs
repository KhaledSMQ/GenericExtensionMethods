using System;

namespace GenericExtensionMethods
{
    /// <summary>
    /// Conversions to and from nullable types.
    /// </summary>
    public static class NullableTypeExtensions
    {
        /// <summary>
        /// Converts an object to a nullable DateTime.
        /// </summary>
        /// <param name="source">Value to convert to nullable DateTime.</param>
        /// <returns>Nullable DateTime.</returns>
        public static DateTime? ToNullableDateTime(this object source)
        {
            if (source.IsNull()) return null;
            if (source is string && ((string)source).IsEmpty()) return null;
            try { return Convert.ToDateTime(source); }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an object to a nullable bool.
        /// </summary>
        /// <param name="source">Value to convert to nullable bool.</param>
        /// <returns>Nullable bool</returns>
        public static bool? ToNullableBoolean(this object source)
        {
            if (source.IsNull()) return null;
            if (source is string && ((string)source).IsEmpty()) return null;
            try { return Convert.ToBoolean(source); }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an object to a nullable Guid.
        /// </summary>
        /// <param name="source">Value to convert to a nullable Guid.</param>
        /// <returns>Nullable Guid</returns>
        public static Guid? ToNullableGuid(this object source)
        {
            if (source.IsNull()) return null;
            if (source is string && ((string)source).IsEmpty()) return null;
            if (source is string)
            {
                try { return new Guid(source as string); }
                catch
                {
                    return null;
                }
            }
            else
            {
                return source as Guid?;
            }
        }

        /// <summary>
        /// Converts object to nullable decimal.
        /// </summary>
        /// <param name="source">Object to convert to Nullable decimal.</param>
        /// <returns>Nullable decimal</returns>
        public static decimal? ToNullableDecimal(this object source)
        {
            if (source.IsNull()) return null;
            if (source is string && ((string)source).IsEmpty()) return null;
            try { return Convert.ToDecimal(source); }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts object to Nullable double
        /// </summary>
        /// <param name="source">Object to convert to Nullable double.</param>
        /// <returns>Nullable double</returns>
        public static double? ToNullableDouble(this object source)
        {
            if (source.IsNull()) return null;
            if (source is string && ((string)source).IsEmpty()) return null;
            try { return Convert.ToDouble(source); }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts object to Nullable int.
        /// </summary>
        /// <param name="source">Object to convert to Nullable int.</param>
        /// <returns>Nullable int</returns>
        public static Int32? ToNullableInt32(this object source)
        {
            if (source.IsNull()) return null;
            if (source is string && ((string)source).IsEmpty()) return null;
            try { return Convert.ToInt32(source); }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts object to Nullable long.
        /// </summary>
        /// <param name="source">Value to convert to Nullable long.</param>
        /// <returns>Nullable long</returns>
        public static Int64? ToNullableInt64(this object source)
        {
            if (source.IsNull()) return null;
            if (source is string && ((string)source).IsEmpty()) return null;
            try { return Convert.ToInt64(source); }
            catch
            {
                return null;
            }
        }
    }
}