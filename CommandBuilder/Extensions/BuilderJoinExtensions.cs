using System;
using CommandBuilder.Configurations;

namespace CommandBuilder.Extensions
{
    public static class BuilderJoinExtensions
    {
        public static SqlCommandBuilder InnerJoin(this SqlCommandBuilder sqlCommandBuilder, string tableName, 
            Action<InnerJoinConfiguration> configuration)
        {
            return sqlCommandBuilder.InnerJoin(new InnerJoinConfiguration(tableName), configuration);
        }

        public static SqlCommandBuilder InnerJoin(this SqlCommandBuilder sqlCommandBuilder, string tableName, string tableAs,
            Action<InnerJoinConfiguration> configuration)
        {
            return sqlCommandBuilder.InnerJoin(new InnerJoinConfiguration(tableName, tableAs), configuration);
        }

        private static SqlCommandBuilder InnerJoin(this SqlCommandBuilder sqlCommandBuilder, 
            InnerJoinConfiguration innerJoinConfiguration, Action<InnerJoinConfiguration> configuration)
        {
            if (innerJoinConfiguration == null)
                throw new ArgumentNullException(nameof(innerJoinConfiguration));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration(innerJoinConfiguration);

            sqlCommandBuilder.SqlBuilder.INNER_JOIN(innerJoinConfiguration.Build());

            return sqlCommandBuilder;
        }
    }
}
