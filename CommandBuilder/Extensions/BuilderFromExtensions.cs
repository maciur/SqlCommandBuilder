using System;

namespace CommandBuilder.Extensions
{
    public static class BuilderFromExtensions
    {
        public static SqlCommandBuilder From(this SqlCommandBuilder sqlCommandBuilder, string tableName, string tableAs = default)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            string fromClause = !string.IsNullOrEmpty(tableAs) ?
                $"{tableName} AS {tableAs.AddSquareBrackets()}" :
                tableName;

            sqlCommandBuilder.SqlBuilder.FROM(fromClause);

            return sqlCommandBuilder;
        }
    }
}