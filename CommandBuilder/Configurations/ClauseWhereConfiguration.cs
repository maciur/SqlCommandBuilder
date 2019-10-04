using System;
using System.Collections.Generic;
using System.Text;
using CommandBuilder.Extensions;

namespace CommandBuilder.Configurations
{
    public class ClauseWhereConfiguration
    {
        protected string Name { get; private set; }
        protected string ColumnPrefix { get; private set; }
        protected object Value { get; private set; }
        protected string Clause { get; private set; }

        public ClauseWhereConfiguration(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            Name = columnName;
        }

        public ClauseWhereConfiguration Prefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentNullException(nameof(prefix));

            ColumnPrefix = prefix
                .AddSquareBrackets();

            return this;
        }

        public ClauseWhereConfiguration Equal<TValue>(TValue value)
        {
            Value = value;
            Clause = "= {0}";
            return this;
        }

        public ClauseWhereConfiguration GreaterThan<TValue>(TValue value)
        {
            Value = value;
            Clause = "> {0}";
            return this;
        }

        public ClauseWhereConfiguration LessThan<TValue>(TValue value)
        {
            Value = value;
            Clause = "< {0}";
            return this;
        }

        public ClauseWhereConfiguration In<TValue>(IEnumerable<TValue> values)
        {
            Value = SQL.List(values);
            Clause = "IN ({0})";
            return this;
        }

        internal void Build(SqlBuilder sqlBuilder)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(ColumnPrefix))
            {
                sb.Append(ColumnPrefix.AddSquareBrackets())
                  .Append(".");
            }

            sb.Append(Name.AddSquareBrackets())
              .Append(" ");

            sb.Append(Clause);

            sqlBuilder.WHERE(sb.ToString(), Value);
        }
    }
}