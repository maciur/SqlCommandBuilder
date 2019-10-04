using System;
using CommandBuilder.Configurations;

namespace CommandBuilder.Extensions
{
    public static class BuilderUpdateExtensions
    {
        public static SqlCommandBuilder Update(this SqlCommandBuilder sqlCommandBuilder, Action<UpdateConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var updateConfiguration = new UpdateConfiguration();
            configuration(updateConfiguration);

            updateConfiguration.Build(sqlCommandBuilder.SqlBuilder);

            return sqlCommandBuilder;
        }
    }
}