using System;
using CommandBuilder.Extensions;

namespace CommandBuilder.Configurations
{
    public class JoinTableConfiguration
    {
        protected string Name { get; set; }
        protected string ColumnPrefix { get; set; }

        public JoinTableConfiguration Column(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            return this;
        }
        
        public JoinTableConfiguration Prefix(string columnPrefix)
        {
            if (string.IsNullOrEmpty(columnPrefix))
                throw new ArgumentNullException(nameof(columnPrefix));

            ColumnPrefix = columnPrefix;
            return this;
        }

        internal string Build()
        {
            if (!string.IsNullOrEmpty(ColumnPrefix))
            {
                var prefix = $"{ColumnPrefix.AddSquareBrackets()}";
                return $"{prefix}.{Name.AddSquareBrackets()}";
            }

            return Name.AddSquareBrackets();
        }
    }
}