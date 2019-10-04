using System;

namespace CommandBuilder.Extensions
{
    internal static class StringExtensions
    {
        internal static string AddSquareBrackets(this string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            if (columnName.Length == 1)
                return $"[{columnName}]";

            if (columnName[0] == '[' && columnName[columnName.Length -1] == ']')
                return columnName;

            return $"[{columnName}]";
        }
    }
}