using System;

namespace CommandBuilder.Extensions
{
    public static class BuilderDeleteExtensions
    {
        public static SqlCommandBuilder Delete(this SqlCommandBuilder sqlCommandBuilder, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            sqlCommandBuilder.SqlBuilder.DELETE_FROM(tableName.AddSquareBrackets());

            return sqlCommandBuilder;
        }
    }
}