using System;
using System.Collections.Generic;
using CommandBuilder.Extensions;

namespace CommandBuilder.Configurations
{
    public class UpdateConfiguration
    {
        protected string TableName { get; private set; }
        protected IDictionary<string, object> SetValues { get; private set; }

        public UpdateConfiguration()
        {
            SetValues = new Dictionary<string, object>();
        }

        public UpdateConfiguration Table(string table)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException(nameof(table));

            TableName = table.AddSquareBrackets();
            return this;
        }
        
        public UpdateConfiguration Set(string columnName, object value)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            SetValues.Add(columnName.AddSquareBrackets(), value);
            return this;
        }

        internal void Build(SqlBuilder sqlBuilder)
        {
            if (sqlBuilder == null)
                throw new ArgumentNullException(nameof(sqlBuilder));

            sqlBuilder.UPDATE(TableName);

            foreach (var setValue in SetValues)
            {
                sqlBuilder.SET($"{setValue.Key} {"= {0}"}", setValue.Value);
            }
        }
    }
}