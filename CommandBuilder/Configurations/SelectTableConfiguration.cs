using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandBuilder.Configurations
{
    public class SelectTableConfiguration
    {
        internal IList<SelectColumnConfiguration> Columns { get; }
        internal string ColumnPrefix { get; private set; }

        public SelectTableConfiguration()
        {
            Columns = new List<SelectColumnConfiguration>();
        }

        public SelectTableConfiguration(string columnPrefix)
            : this()
        {
            if (string.IsNullOrEmpty(columnPrefix))
                throw new ArgumentNullException(nameof(columnPrefix));

            ColumnPrefix = columnPrefix;
        }

        public SelectColumnConfiguration Column(string name)
        {
            var column = new SelectColumnConfiguration(name);
            Columns.Add(column);
            return column;
        }

        internal string Build()
        {
            return string.Join(", ", Columns.Select(x => x.Build(ColumnPrefix)));
        }
    }
}