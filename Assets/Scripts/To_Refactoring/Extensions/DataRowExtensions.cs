using System;
using System.Data;
using System.Linq;

namespace Extensions
{
    public static class DataRowExtensions
    {
        public static int GetInt(this DataRow row, int column)
        {
            return Convert.ToInt32(row[column]);
        }

        public static string GetString(this DataRow row, int column)
        {
            return Convert.ToString(row[column]);
        }

        public static float GetFloat(this DataRow row, int column)
        {
            return Convert.ToSingle(row[column]);
        }

        public static int GetInt(this DataRow row, string column)
        {
            return Convert.ToInt32(row[column]);
        }

        public static string GetString(this DataRow row, string column)
        {
            return Convert.ToString(row[column]);
        }

        public static float GetFloat(this DataRow row, string column)
        {
            return Convert.ToSingle(row[column]);
        }

        public static bool IsEmpty(this DataRow row)
        {
            return row == null || row.ItemArray.All(i => i.IsNullEquivalent());
        }

        public static bool IsNullEquivalent(this object value)
        {
            return value == null
                   || value is DBNull
                   || string.IsNullOrWhiteSpace(value.ToString());
        }
    }
}