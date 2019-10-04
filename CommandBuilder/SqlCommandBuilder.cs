using CommandBuilder.Extensions;
using Dapper;
using System;
using System.Data;

namespace CommandBuilder
{
    public class SqlCommandBuilder
    {
        internal SqlBuilder SqlBuilder { get; }

        private SqlCommandBuilder(SqlBuilder sqlBuilder)
        {
            SqlBuilder = sqlBuilder
                ?? throw new ArgumentNullException(nameof(sqlBuilder));
        }

        public SqlCommandBuilder()
        {
            SqlBuilder = new SqlBuilder();
        }
        
        public SqlCommandBuilder Clone()
        {
            return new SqlCommandBuilder(SqlBuilder.Clone());
        }

        public CommandDefinition DapperCommand(IDbTransaction dbTransaction = null)
        {
            return SqlBuilder.ToDapperCommand(dbTransaction);
        }
    }
}