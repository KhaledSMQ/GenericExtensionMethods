/*
 * Copyright 2009, Payton Byrd
 * Licensed Under the Microsoft Public License (MS-PL)
 */

using System;

namespace GenericExtensionMethods
{
    /// <summary>
    /// Collection of extension methods
    /// relating to DBNull.
    /// </summary>
    public static class DBNullExtensions
    {
        /// <summary>
        /// Returns the supplied alternative value
        /// if the source is DBNull.Value.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="value">Source value.</param>
        /// <param name="alternative">Alternate value.</param>
        /// <returns>Source value or alternate value if source 
        /// is DBNull.Value.</returns>
        public static T CoalesceDBNull<T>(
            this object value,
            T alternative)
        {
            return (value is DBNull ? alternative : (T)value);
        }

        /// <summary>
        /// Returns default(T) if the source is
        /// DBNull.Value.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="value">Source value.</param>
        /// <returns>Source value or default(T) if source 
        /// is DBNull.Value.</returns>
        public static T CoalesceDBNull<T>(
            this object value)
        {
            return value.CoalesceDBNull(default(T));
        }
    }
}
