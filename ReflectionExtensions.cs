/*
 * Copyright 2009, Payton Byrd
 * Licensed Under the Microsoft Public License (MS-PL)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericExtensionMethods
{
    // Payton Byrd - 2010/01/19
    // All commented code will be removed for version 1.0.0

    /// <summary>
    /// Collection of Extension Methods
    /// relating to Reflection operations of
    /// types.
    /// </summary>
    public static class ReflectionExtensions
    {
        #region CreateInstance

        /// <summary>
        /// Creates an instance of the generic
        /// type specified using the parameters
        /// specified.
        /// </summary>
        /// <typeparam name="T">The type to
        /// instantiate.</typeparam>
        /// <param name="type">The System.Type 
        /// being instantiated.</param>
        /// <param name="parameters">The array
        /// of parameters to use when calling 
        /// the constructor.</param>
        /// <returns>An instance of the specified 
        /// type.</returns>
        /// <exception cref="System.Exception" />
        /// <remarks>
        /// If there is not a constructor that 
        /// matches the parameters then an 
        /// <see cref="System.Exception"/> is
        /// thrown.
        /// </remarks>
        /// <example>
        /// typeof(MyObject).CreateInstance(
        ///    new object[] { 1, 3.0M, "Final Parameter" });
        /// </example>
        public static T CreateInstance<T>(
            this Type type,
            object[] parameters
            )
        {
            parameters.AssertParameterNotNull(
                "The parameters array must not be null.",
                "parameters");

            type.AssertEquals<Exception>(
                typeof(T),
                "The generic type must match the type instance.");

            T result;

            var types = new List<Type>();

            foreach (object parameter in parameters)
            {
                if (parameter.IsNotNull())
                {
                    types.Add(parameter.GetType());
                }
                else
                {
                    types.Add(typeof(object));
                }
            }

            ConstructorInfo ctor =
                type.GetConstructor(types.ToArray());

            if (ctor.IsNotNull())
            {
                result = (T)ctor.Invoke(parameters);
            }
            else
            {
                throw new ArgumentException(
                    "There are no constructors for " +
                    type.FullName + " that " +
                    "match the types provided.");
            }

            return result;
        }

        /// <summary>
        /// Creates an instance of the generic
        /// type specified using the default
        /// constructor.
        /// </summary>
        /// <typeparam name="T">The type to
        /// instantiate.</typeparam>
        /// <param name="type">The System.Type 
        /// being instantiated.</param>
        /// <returns>An instance of the specified 
        /// type.</returns>
        /// <example>
        /// typeof(MyObject).CreateInstance();
        /// </example>
        public static T CreateInstance<T>(
            this Type type)
            where T : new()
        {
            type.AssertEquals<Exception>(
                typeof(T),
                "The generic type must match the type instance.");

            return Activator.CreateInstance<T>();
        }

        #endregion

        /// <summary>
        /// Populates common properties of the target from the source.
        /// </summary>
        /// <param name="source">The object containing the source values.</param>
        /// <param name="target">The object to populate.</param>
        /// <param name="flags">BindingFlags of the operation.</param>
        public static void PopulateInto(
            this object source,
            object target,
            BindingFlags flags)
        {
            foreach (var pi in source.GetType().GetProperties(flags))
            {
                var tpi = target.GetProperty(pi.Name, flags);

                if (tpi.IsNotNull())
                {
                    try
                    {
                        object originalValue;
                        object value = originalValue = source.GetValue<object>(pi.Name, new object[0], flags);
                        try
                        {
                            value = Convert.ChangeType(originalValue, tpi.PropertyType);
                        }
                        catch
                        {
                        }
                        tpi.SetValue(target, value, new object[0]);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a property from the object.
        /// </summary>
        /// <param name="source">The object containing the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="flags">The BindingFlags to use to
        /// get the property.</param>
        /// <returns>An instance of PropertyInfo for the
        /// specified property.</returns>
        public static PropertyInfo GetProperty(
            this object source,
            string name,
            BindingFlags flags)
        {
            source.AssertParameterNotNull(
                "Cannot get properties of null object.",
                "source");

            name.AssertParameterNotNull(
                "Cannot get property when name is null.",
                "name");

            name.AssertParameterNotEmpty(
                "Cannot get property when name is empty.",
                "name");

            PropertyInfo result = (from property in source.GetType().GetProperties(flags)
                                   where property.Name == name
                                   select property).FirstOrDefault();

            return result;
        }


        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string PropertyName<T>(
            this Expression<Func<T>> source)
        {
            var memberExpression =
                (MemberExpression)source.Body;

            string result = memberExpression.Member.Name;

            result.AssertParameterNotNull(
                "Major problem.  The name of the member cannot be obtained.",
                "source");

            return result;
        }

        /// <summary>
        /// Gets a list of custom attributes
        /// for the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static List<Attribute> GetCustomAttributes<T>(
            this Expression<Func<T>> source,
            Type sourceType)
        {
            var result = new List<Attribute>();

            string name = source.PropertyName();

            object[] attributes =
                sourceType.GetProperty(name).GetCustomAttributes(true);

            result.AddRange(
                from attribute in attributes
                where (attribute.GetType() == typeof(Attribute))
                select attribute as Attribute);

            return result;
        }

        /// <summary>
        /// Gets the value of the specified property
        /// using reflection.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="source">The object containing the value to return.</param>
        /// <param name="name">The name of the property to return.</param>
        /// <param name="indices">The ordinals of the collection keys to return.</param>
        /// <param name="flags">The binding flags to use.</param>
        /// <returns>The typed value.</returns>
        public static T GetValue<T>(
            this object source,
            string name,
            object[] indices,
            BindingFlags flags)
        {
            PropertyInfo pi = source.GetProperty(name, flags);

            return (T)(pi.GetValue(source, indices));
        }

        /// <summary>
        /// Gets the value of the specified proeprty
        /// using reflection.  Returns default(T) if
        /// the value is not initialized.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="source">The object containing the value to return.</param>
        /// <param name="name">The name of the property to return.</param>
        /// <param name="indices">The ordinals of the collection keys to return.</param>
        /// <param name="flags">The binding flags to use.</param>
        /// <returns>The typed value or default for the return type.</returns>
        public static T GetValueOrDefault<T>(
            this object source,
            string name,
            object[] indices,
            BindingFlags flags)
        {
            PropertyInfo pi = source.GetProperty(name, flags);

            if (pi.IsNotNull())
            {
                return (T)(pi.GetValue(source, indices));
            }

            return default(T);
        }

        public static bool IsNullOrDefault<T>(this T source)
        {
            return source.IsNull() || source.IsDefault();
        }

        public static bool IsNotNullOrDefault<T>(this T source)
        {
            return !source.IsNullOrDefault();
        }

        /// <summary>
        /// Tests the source for default(T)
        /// where T is the type of the source.
        /// </summary>
        /// <typeparam name="T">Type for testing.</typeparam>
        /// <param name="source">Value to test.</param>
        /// <returns>True if the value is default(T).</returns>
        public static bool IsDefault<T>(this T source)
        {
            return source.Equals(default(T));
        }

        /// <summary>
        /// Tests the source for not being
        /// default(T) where T is the type of 
        /// the source.
        /// </summary>
        /// <typeparam name="T">Type for testing.</typeparam>
        /// <param name="source">Value to test.</param>
        /// <returns>True if the value is not default(T).</returns>
        public static bool IsNotDefault<T>(this T source)
        {
            return !source.IsDefault();
        }

    }
}