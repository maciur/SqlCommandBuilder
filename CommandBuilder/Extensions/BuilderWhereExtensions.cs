using System;
using CommandBuilder.Configurations;

namespace CommandBuilder.Extensions
{
    public static class BuilderWhereExtensions
    {
        public static SqlCommandBuilder Where(this SqlCommandBuilder sqlCommandBuilder, Action<WhereConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var whereConfiguration = new WhereConfiguration();

            configuration(whereConfiguration);

            whereConfiguration.Build(sqlCommandBuilder.SqlBuilder);

            return sqlCommandBuilder;
        }
    }
}
