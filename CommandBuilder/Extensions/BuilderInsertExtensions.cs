using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using CommandBuilder.Configurations;

namespace CommandBuilder.Extensions
{
    public static class BuilderInsertExtensions
    {
        public static SqlCommandBuilder InsertInto(this SqlCommandBuilder sqlCommandBuilder,
            Action<InsertConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var insertConfiguration = new InsertConfiguration();
            configuration(insertConfiguration);

            sqlCommandBuilder.SqlBuilder.INSERT_INTO(insertConfiguration.Build());

            return sqlCommandBuilder;
        }

        public static CommandDefinition InsertInto<TEntity>(this SqlCommandBuilder sqlCommandBuilder, string tableName,
            TEntity entity, IDbTransaction transaction = null)
            where TEntity : class
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var insertConfiguration = new InsertConfiguration();
            insertConfiguration.Table(tableName);
            var insertValuesConfiguration = new InsertValuesConfiguration();

            foreach (var property in entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                insertConfiguration.Column(property.Name);
                insertValuesConfiguration.Value(property.GetValue(entity));
            }

            sqlCommandBuilder.SqlBuilder.INSERT_INTO(insertConfiguration.Build());
            sqlCommandBuilder.SqlBuilder.VALUES(insertValuesConfiguration.Values);

            return sqlCommandBuilder.SqlBuilder.ToDapperCommand(transaction);
        }

        public static CommandDefinition InsertInto<TEntity>(this SqlCommandBuilder sqlCommandBuilder, string tableName,
            IEnumerable<TEntity> entities, IDbTransaction transaction = null) where TEntity : class
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities));

            var entity = entities.First();

            var entityProperties = entity.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var propertiesNames = entityProperties.Select(x => x.Name);

            var sb = new StringBuilder();
            sb.Append(tableName.AddSquareBrackets());
            sb.Append("(");
            sb.Append(string.Join(",", propertiesNames.Select(x => x.AddSquareBrackets())));
            sb.Append(") VALUES(");
            sb.Append(string.Join(",", propertiesNames.Select(x => $"@{x}")));
            sb.Append(")");

            return new CommandDefinition(sb.ToString(), parameters: entities, transaction: transaction);
        }

        public static SqlCommandBuilder Values(this SqlCommandBuilder sqlCommandBuilder,
            Action<InsertValuesConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var insertValuesConfiguration = new InsertValuesConfiguration();

            configuration(insertValuesConfiguration);
            sqlCommandBuilder.SqlBuilder.VALUES(insertValuesConfiguration.Values);

            return sqlCommandBuilder;
        }
    }
}