using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TeacherAppreciation.Infrastructure
{
    public static class Utilities
    {
        /// <summary>
        /// Convert DataTable to List<list type="T"></list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();

                foreach (var pro in properties)
                {
                    if (!columnNames.Contains(pro.Name)) continue;
                    var value = row[pro.Name] == DBNull.Value ? "" : row[pro.Name];
                    pro.SetValue(objT, value);
                }

                return objT;
            }).ToList();

        }

        /// <summary>
        /// Check string parameter for a null value and throw ArgumentNullException if so
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static T CheckNotNull<T>(T value, string paramName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
            return value;
        }

        /// <summary>
        /// Check string[] array for 0 count and throw ArgumentNullException if so
        /// </summary>
        /// <param name="rowCells"></param>
        /// <returns></returns>
        public static object[] CheckNotNull(string[] rowCells)
        {
            if (rowCells.Length == 0)
                throw new ArgumentNullException(nameof(rowCells));
            return rowCells;
        }
    }
}