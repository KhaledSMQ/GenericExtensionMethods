/*
 * Copyright 2009, Payton Byrd
 * Licensed Under the Microsoft Public License (MS-PL)
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericExtensionMethods
{
    // Payton Byrd - 2010/01/19
    // All commented code will be removed for version 1.0.0

    /// <summary>
    /// A collection of extension methods
    /// used to perform operations on 
    /// collections.
    /// </summary>
    public static class DataSetExtensions
    {
        /// <summary>
        /// Creates a new DataRow and adds it
        /// to the source DataTable for the supplied
        /// object.
        /// </summary>
        /// <typeparam name="T">Typed DataRow</typeparam>
        /// <param name="source">Object to add to the DataTable.</param>
        /// <param name="table">DataTable to add the row to.</param>
        /// <returns></returns>
        public static T AsDataRow<T>(
            this object source,
            DataTable table)
            where T : DataRow
        {
            source.AssertParameterNotNull(
                "Cannot convert a null source to a DataRow.",
                "source");

            table.AssertParameterNotNull(
                "Cannot add a DataRow to a null DataTable.",
                "table");

            T result = null;

            List<DataColumn> columns =
                MakeDataColumns(source);

            if (columns.IsNotEmpty())
            {
                table.AddColumnsToDataTable(columns);
                result = (T) table.NewRow();
                result.PopulateDataRow(source, columns);
                table.Rows.Add(result);
            }

            return result;
        }

        /// <summary>
        /// Helper method
        /// </summary>
        /// <param name="row"></param>
        /// <param name="source"></param>
        /// <param name="columns"></param>
        private static void PopulateDataRow(
            this DataRow row,
            object source,
            List<DataColumn> columns)
        {
            row.AssertParameterNotNull(
                "Cannot populate a null DataRow",
                "row");

            source.AssertParameterNotNull(
                "Cannot add values to a DataRow from a null source.",
                "source");

            columns.AssertParameterNotNull(
                "The list of DataColumns is null.",
                "columns");

            if (columns.IsEmpty())
            {
                throw new ArgumentOutOfRangeException(
                    "columns",
                    "The list of DataColumns is empty.");
            }

            foreach (DataColumn column in columns)
            {
                string propertyName = column.Caption;
                bool readOnly = column.ReadOnly;

                column.ReadOnly = false;

                if (propertyName.IsNullOrEmpty())
                {
                    propertyName = column.ColumnName;
                }

                PropertyInfo property =
                    source.GetProperty(propertyName,
                                       BindingFlags.Instance |
                                       BindingFlags.Public);

                if (property.IsNotNull()
                    && property.CanRead)
                {
                    row[column] = property.GetValue(
                        source, new object[0]);
                }

                column.ReadOnly = readOnly;
            }
        }

        /// <summary>
        /// Adds a list of columns to a DataTable.
        /// </summary>
        /// <param name="table">The DataTable to add
        /// the columns to.</param>
        /// <param name="columns">The list of columns
        /// to add to the table.</param>
        /// <remarks>
        /// This method will add unmatched columns,
        /// skip redundant columns (same name and type)
        /// and add additional columns with a counter
        /// for mismatched naming collisions.
        /// </remarks>
        public static void AddColumnsToDataTable(
            this DataTable table,
            List<DataColumn> columns)
        {
            table.AssertParameterNotNull(
                "Cannot add columns to a null DataTable.",
                "table");

            columns.AssertParameterNotNull(
                "The list of DataColumns is null.",
                "columns");

            if (columns.IsEmpty())
            {
                throw new ArgumentOutOfRangeException(
                    "columns",
                    "The list of DataColumns is empty.");
            }

            List<DataColumn> tableColumns =
                table.Columns.ToList();

            var replaceList =
                new Dictionary<DataColumn, DataColumn>();

            foreach (DataColumn column in columns)
            {
                List<DataColumn> matchingColumns =
                    (from tableColumn in tableColumns
                     where tableColumn.ColumnName.StartsWith(column.ColumnName)
                           && tableColumn.DataType.Equals(column.DataType)
                     select tableColumn).ToList();

                List<DataColumn> misMatchedColumns =
                    (from tableColumn in tableColumns
                     where tableColumn.ColumnName.StartsWith(column.ColumnName)
                           && !tableColumn.DataType.Equals(column.DataType)
                     select tableColumn).ToList();

                if (matchingColumns.IsEmpty() &&
                    misMatchedColumns.IsEmpty())
                {
                    tableColumns.Add(column);
                    table.Columns.Add(new DataColumn(
                                          column.ColumnName, column.DataType, column.Expression));
                }
                else if (misMatchedColumns.IsNotEmpty())
                {
                    var newColumn = new DataColumn(
                        column.ColumnName, column.DataType, column.Expression)
                                        {
                                            Caption = column.ColumnName,
                                            ColumnName = column.ColumnName + misMatchedColumns.Count
                                        };

                    tableColumns.Add(column);
                    table.Columns.Add(newColumn);
                }
                else
                {
                    replaceList.Add(column,
                                    matchingColumns.FirstOrDefault());
                }
            }

            foreach (DataColumn column in replaceList.Keys)
            {
                columns.Remove(column);
                columns.Add(replaceList[column]);
            }
        }

        /// <summary>
        /// Helper method.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static List<DataColumn> MakeDataColumns(object source)
        {
            source.AssertParameterNotNull(
                "Cannot make DataColumns from a null source.",
                "source");

            var result = new List<DataColumn>();

            IEnumerable<PropertyInfo> properties =
                from propertyInfo in source.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance)
                where propertyInfo.CanRead
                select propertyInfo;

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead)
                {
                    var column = new DataColumn(
                        property.Name, property.PropertyType);

                    column.ReadOnly = !property.CanWrite;

                    result.Add(column);
                }
            }

            return result;
        }

        /// <summary>
        /// Mades a List&lt;DataColumn&gt; of the 
        /// DataColumns of the DataColumnCollection.
        /// </summary>
        /// <param name="source">The list of columns to convert.</param>
        /// <returns>Strongly typed list of DataColumn objects.</returns>
        /// <example>
        /// <code>
        /// var table = SomeMethodThatReturnsTable();
        /// 
        /// // List&lt;DataColumn&gt;
        /// var columnsList = table.Columns.ToList();
        /// </code>
        /// </example>
        public static List<DataColumn> ToList(
            this DataColumnCollection source)
        {
            var tableColumns =
                new List<DataColumn>();

            foreach (DataColumn tableColumn in source)
            {
                tableColumns.Add(tableColumn);
            }

            return tableColumns;
        }
    }
}