using System;
using System.Collections.Generic;

namespace CommandBuilder.Configurations
{
    public class SelectConfiguration
    {
        protected IList<SelectTableConfiguration> ColumnsConfigurations { get; }

        public SelectConfiguration()
        {
            ColumnsConfigurations = new List<SelectTableConfiguration>();
        }

        public SelectConfiguration Table(Action<SelectTableConfiguration> configuration)
        {
            return Table(new SelectTableConfiguration(), configuration);
        }

        public SelectConfiguration Table(string columnPrefix, Action<SelectTableConfiguration> configuration)
        {
            return Table(new SelectTableConfiguration(columnPrefix), configuration);
        }

        protected SelectConfiguration Table(SelectTableConfiguration selectTableConfiguration,
            Action<SelectTableConfiguration> configuration)
        {
            if (selectTableConfiguration == null)
                throw new ArgumentNullException(nameof(selectTableConfiguration));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration(selectTableConfiguration);
            ColumnsConfigurations.Add(selectTableConfiguration);

            return this;
        }

        internal void Build(SqlBuilder sqlBuilder)
        {
            foreach (var columns in ColumnsConfigurations)
            {
                sqlBuilder.SELECT(columns.Build());
            }
        }
    }
}