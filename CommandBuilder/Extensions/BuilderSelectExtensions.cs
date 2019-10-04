using System;
using CommandBuilder.Configurations;

namespace CommandBuilder.Extensions
{
    public static class BuilderSelectExtensions
    {
        public static SqlCommandBuilder Select(this SqlCommandBuilder sqlCommandBuilder, Action<SelectConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var selectConfiguration = new SelectConfiguration();

            configuration(selectConfiguration);

            selectConfiguration.Build(sqlCommandBuilder.SqlBuilder);

            return sqlCommandBuilder;
        }
    }
}
