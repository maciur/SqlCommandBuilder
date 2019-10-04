using System;
using System.Text;
using CommandBuilder.Extensions;

namespace CommandBuilder.Configurations
{
    public class InnerJoinConfiguration
    {
        protected JoinTableConfiguration Left { get; private set; }
        protected JoinTableConfiguration Right { get; private set; }
        protected string Table { get; }
        protected string TableAs { get; }

        public InnerJoinConfiguration(string table)
        {
            if (string.IsNullOrEmpty(table))
                throw new ArgumentNullException(nameof(table));

            Table = table;
        } 
        
        public InnerJoinConfiguration(string table, string tableAs) 
            : this(table)
        {
            if (string.IsNullOrEmpty(tableAs))
                throw new ArgumentNullException(nameof(tableAs));

            TableAs = tableAs;
        }

        public InnerJoinConfiguration LeftSide(Action<JoinTableConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            Left = new JoinTableConfiguration();
            configuration(Left);

            return this;
        }
        
        public InnerJoinConfiguration RightSide(Action<JoinTableConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            Right = new JoinTableConfiguration();
            configuration(Right);

            return this;
        }

        internal string Build()
        {
            var sb = new StringBuilder();
            sb.Append(Table);

            if (!string.IsNullOrEmpty(TableAs))
            {
                sb.Append($" AS ").Append(TableAs.AddSquareBrackets());
            }

            return sb.Append(" ON ")
              .Append(Left.Build())
              .Append(" = ")
              .Append(Right.Build())
              .ToString();
        }
    }
}