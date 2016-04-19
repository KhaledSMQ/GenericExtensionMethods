/*
 * Copyright 2009, Payton Byrd
 * Licensed Under the Microsoft Public License (MS-PL)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace GenericExtensionMethods
{
    /// <summary>
    /// A collection of Extension Methods
    /// relating to validation of values.
    /// </summary>
    public static class ValidationExtensions
    {
        #region Generic

        /// <summary>
        /// Tests if the object is null.
        /// </summary>
        /// <param name="value">The object to test.</param>
        /// <returns>True if the object is null.</returns>
        [DebuggerNonUserCode]
        public static bool IsNull(this object value)
        {
            return value == null;
        }

        /// <summary>
        /// Tests if the object is not null.
        /// </summary>
        /// <param name="value">The object to test.</param>
        /// <returns>True if the object is not null.</returns>
        [DebuggerNonUserCode]
        public static bool IsNotNull(this object value)
        {
            return value != null;
        }

        /// <summary>
        /// Throws an <see cref="System.ArgumentNullException"/> 
        /// if the the value is null.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="message">The message to display if the value is null.</param>
        /// <param name="name">The name of the parameter being tested.</param>
        [DebuggerNonUserCode]
        public static void AssertParameterNotNull(
            this object value, string message, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name, message);
        }

        /// <summary>
        /// Throws an <see cref="System.ArgumentException"/>
        /// if the string value is empty.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="message">The message to display if the value is null.</param>
        /// <param name="name">The name of the parameter being tested.</param>
        [DebuggerNonUserCode]
        public static void AssertParameterNotEmpty(
            this string value, string message, string name)
        {
            value.AssertParameterNotNull(message, name);

            if (value.IsEmpty())
                throw new ArgumentException(message, name);
        }

        /// <summary>
        /// Throws the specified exception if the
        /// values are not equal.
        /// </summary>
        /// <typeparam name="TExceptionType">
        /// The type of exception to throw.</typeparam>
        /// <param name="value">The value to test.</param>
        /// <param name="compareTo">The expected value.</param>
        /// <param name="message">The message to display if the value is null.</param>
        [DebuggerNonUserCode]
        public static void AssertEquals<TExceptionType>(
            this object value, object compareTo, string message)
            where TExceptionType : Exception
        {
            if (!value.Equals(compareTo))
            {
                if (message.IsNullOrEmpty())
                {
                    message = string.Format(
                        "{0} does not equal expected value of {1}",
                        value, compareTo);
                }

                var e =
                    typeof (TExceptionType).CreateInstance<TExceptionType>(
                        new object[] {message});

                if (e.IsNull())
                {
                    throw new Exception(
                        "Cannot instantiate the expected exception.");
                }

                throw e;
            }
        }

        #endregion

        #region Strings

        /// <summary>
        /// Tests if the string is empty.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>True if the string is empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsEmpty(this string value)
        {
            return value.Trim().Length == 0;
        }

        /// <summary>
        /// Tests if the string is not empty.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>True if the string is not empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsNotEmpty(this string value)
        {
            return value.Trim().Length > 0;
        }

        /// <summary>
        /// Tests if the string is null or empty.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>True if the string is null or empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsNullOrEmpty(this string value)
        {
            return value == null || value.Trim().Length == 0;
        }

        #endregion Strings

        #region Collections

        #region IsEmpty

        /// <summary>
        /// Tests if the collection is empty.
        /// </summary>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsEmpty(this ICollection collection)
        {
            collection.AssertParameterNotNull(
                "The collection cannot be null.",
                "collection");

            return collection.Count == 0;
        }

        /// <summary>
        /// Tests if the collection is empty.
        /// </summary>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsEmpty(this IDictionary collection)
        {
            collection.AssertParameterNotNull(
                "The collection cannot be null.",
                "collection");

            return collection.Count == 0;
        }

        /// <summary>
        /// Tests if the IDictionary is empty.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of 
        /// the IDictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values
        /// of the IDictionary.</typeparam>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsEmpty<TKey, TValue>(
            this IDictionary<TKey, TValue> collection)
        {
            collection.AssertParameterNotNull(
                "The collection cannot be null.",
                "collection");

            return collection.Count == 0;
        }

        /// <summary>
        /// Tests if the 
        /// <see cref="System.Data.DataTable"/>
        /// is empty.
        /// </summary>
        /// <param name="table">The table to test.</param>
        /// <returns>True if the table is empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsEmpty(
            this DataTable table)
        {
            table.AssertParameterNotNull(
                "The table cannot be null.",
                "table");

            return table.Rows.Count == 0;
        }

        #endregion IsEmpty

        #region IsNotEmpty

        /// <summary>
        /// Tests if the collection is not empty.
        /// </summary>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is not empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsNotEmpty(this ICollection collection)
        {
            collection.AssertParameterNotNull(
                "The collection cannot be null.",
                "collection");

            return collection.Count > 0;
        }

        /// <summary>
        /// Tests if the collection is not empty.
        /// </summary>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is not empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsNotEmpty(this IDictionary collection)
        {
            collection.AssertParameterNotNull(
                "The collection cannot be null.",
                "collection");

            return collection.Count > 0;
        }

        /// <summary>
        /// Tests if the IDictionary is not empty.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of 
        /// the IDictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values
        /// of the IDictionary.</typeparam>
        /// <param name="collection">The collection to test.</param>
        /// <returns>True if the collection is not empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsNotEmpty<TKey, TValue>(
            this IDictionary<TKey, TValue> collection)
        {
            collection.AssertParameterNotNull(
                "The collection cannot be null.",
                "collection");

            return collection.Count > 0;
        }

        /// <summary>
        /// Tests if the 
        /// <see cref="System.Data.DataTable"/>
        /// is not empty.
        /// </summary>
        /// <param name="table">The table to test.</param>
        /// <returns>True if the table is not empty.</returns>
        [DebuggerNonUserCode]
        public static bool IsNotEmpty(
            this DataTable table)
        {
            table.AssertParameterNotNull(
                "The table cannot be null.",
                "table");

            return table.Rows.Count > 0;
        }

        #endregion IsNotEmpty

        #endregion Collections
    }
}