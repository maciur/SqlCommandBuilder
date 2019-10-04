using System;
using System.Collections.Generic;

namespace CommandBuilder.Configurations
{
    public class WhereConfiguration
    {
        protected IList<ClauseWhereConfiguration> Clauses { get; }

        public WhereConfiguration()
        {
            Clauses = new List<ClauseWhereConfiguration>();
        }

        public ClauseWhereConfiguration Clause(string columnName, Action<ClauseWhereConfiguration> action)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentNullException(nameof(columnName));

            var whereClause = new ClauseWhereConfiguration(columnName);
            action(whereClause);
            Clauses.Add(whereClause);
            return whereClause;
        }

        internal void Build(SqlBuilder sqlBuilder)
        {
            foreach (var clause in Clauses)
            {
                clause.Build(sqlBuilder);
            }
        }
    }
}