using System;
using CommandBuilder.Extensions;

namespace CommandBuilder.Configurations
{
    public class SelectColumnConfiguration
    {
        protected string Name { get; private set; }
        protected string SelectAs { get; private set; }

        internal SelectColumnConfiguration(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            Name = columnName != "*" ?
                columnName.AddSquareBrackets() :
                columnName;
        }

        public SelectColumnConfiguration As(string selectAs)
        {
            if (string.IsNullOrEmpty(selectAs))
                throw new ArgumentNullException(nameof(selectAs));

            SelectAs = selectAs.AddSquareBrackets();
            return this;
        }

        internal string Build(string prefix)
        {
            string columnName = !string.IsNullOrEmpty(SelectAs) ?
                $"{Name} AS {SelectAs.AddSquareBrackets()}" : Name;

            if (!string.IsNullOrEmpty(prefix))
            {
                prefix = prefix.AddSquareBrackets();
                columnName = $"{prefix}.{columnName}";
            }

            return columnName;
        }
    }
}