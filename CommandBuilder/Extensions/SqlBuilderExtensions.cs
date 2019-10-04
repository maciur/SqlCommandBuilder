using Dapper;
using System;
using System.Data;
using System.Globalization;

namespace CommandBuilder.Extensions
{
    public static class SqlBuilderExtensions
    {
        public static CommandDefinition ToDapperCommand(this SqlBuilder sqlBuilder, IDbTransaction transaction = null)
        {
            if (sqlBuilder == null)
                throw new ArgumentNullException(nameof(sqlBuilder));

            var commandText = sqlBuilder.ToString();
            var parameters = sqlBuilder.ParameterValues;

            if (parameters == null || parameters.Count == 0)
            {
                return new CommandDefinition(commandText, transaction: transaction);
            }

            object[] paramPlaceholders = new object[parameters.Count];
            DynamicParameters dynamicParameters = new DynamicParameters();
            for (int i = 0; i < paramPlaceholders.Length; i++)
            {
                object paramValue = parameters[i];
                var parameterName = $"@p{i.ToString(CultureInfo.InvariantCulture)}";
                dynamicParameters.Add(parameterName, paramValue);
                paramPlaceholders[i] = parameterName;
            }

            commandText = string.Format(CultureInfo.InvariantCulture, commandText, paramPlaceholders);
            return new CommandDefinition(commandText, parameters: dynamicParameters, transaction: transaction);
        }
    }
}