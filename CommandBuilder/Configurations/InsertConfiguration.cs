using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandBuilder.Extensions;

namespace CommandBuilder.Configurations
{
    public class InsertConfiguration
    {
        protected string TableName { get; private set; }
        protected IList<string> Columns { get; private set; }

        public InsertConfiguration()
        {
            Columns = new List<string>();
        }

        public InsertConfiguration Table(string table)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException(nameof(table));

            TableName = table.AddSquareBrackets();
            return this;
        }
        
        public InsertConfiguration Column(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            Columns.Add(columnName);
            return this;
        }

        internal string Build()
        {
            var sb = new StringBuilder();
            sb.Append(TableName.AddSquareBrackets());
            sb.Append("(");
            sb.Append(string.Join(",", Columns.Select(x => x.AddSquareBrackets())));
            sb.Append(")");
            return sb.ToString();
        }
    }
}