using CommandBuilder.Extensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace CommandBuilder.Tests
{
    public static class CommandBuilderTestCases
    {
        public static TestCaseData[] Selectors { get; }
        public static TestCaseData[] Insertions { get; }
        public static TestCaseData[] Updates { get; }
        public static TestCaseData[] Delete { get; }

        static CommandBuilderTestCases()
        {
            Selectors = new []
            {
                CreateTestCase(x => x.Select(y => y.Table(z=>z.Column("*"))).From("TestTable"),
                    $"SELECT *{Environment.NewLine}FROM [TestTable]" , "Select Test 1"),

                CreateTestCase(x => x.Select(y => y.Table("tt", z=>z.Column("Id"))).From("TestTable", "tt"),
                    $"SELECT [tt].[Id]{Environment.NewLine}FROM [TestTable] AS [tt]", "Select Test 2"),

                CreateTestCase(x => x
                        .Select(y => y
                            .Table("a", z => z.Column("Id"))
                            .Table("b", z => z.Column("Name")))
                        .From("Login", "a")
                        .InnerJoin("Users", "b", y =>
                        {
                            y.LeftSide(z => z.Prefix("a").Column("Id"));
                            y.RightSide(z => z.Prefix("b").Column("Id"));
                        }),
                    $"SELECT [a].[Id], [b].[Name]{Environment.NewLine}" +
                    $"FROM [Login] AS [a]{Environment.NewLine}INNER JOIN [Users] AS [b] ON " +
                    $"[a].[Id] = [b].[Id]", "Select Test 3"),
                
                CreateTestCase(x => x
                        .Select(y => y
                            .Table("[a]", z => z.Column("Id"))
                            .Table("b", z => z.Column("[Name]")))
                        .From("Login", "[a]")
                        .InnerJoin("Users", "[b]", y =>
                        {
                            y.LeftSide(z => z.Prefix("a").Column("[Id]"));
                            y.RightSide(z => z.Prefix("[b]").Column("Id"));
                        }),
                    $"SELECT [a].[Id], [b].[Name]{Environment.NewLine}" +
                    $"FROM [Login] AS [a]{Environment.NewLine}INNER JOIN [Users] AS [b] ON " +
                    $"[a].[Id] = [b].[Id]", "Select Test 4"),
                
                CreateTestCase(x => x
                        .Select(y => y
                            .Table("[a]", z => z.Column("Id"))
                            .Table("b", z => z.Column("[Name]")))
                        .From("Login", "[a]")
                        .InnerJoin("Users", "[b]", y =>
                        {
                            y.LeftSide(z => z.Prefix("a").Column("[Id]"));
                            y.RightSide(z => z.Prefix("[b]").Column("Id"));
                        })
                        .Where(y => y.Clause("Id", z => z.Prefix("a").Equal(1))),
                    $"SELECT [a].[Id], [b].[Name]{Environment.NewLine}" +
                    $"FROM [Login] AS [a]{Environment.NewLine}INNER JOIN [Users] AS [b] ON " +
                    $"[a].[Id] = [b].[Id]{Environment.NewLine}WHERE [a].[Id] = @p0", "Select Test 5"),
            };

            Insertions = new[]
            {
                CreateTestCase(x => x.InsertInto(y=>y.Table("Users").Column("Id").Column("Name")).Values(y=>y.Value(1).Value("John")), 
                    $"INSERT INTO [Users]([Id],[Name]){Environment.NewLine}VALUES (@p0, @p1)", "Insert Test 1")
            };
            
            Updates = new[]
            {
                CreateTestCase(x => x.Update(y=>y.Table("Users").Set("Name", "John")).Where(y=>y.Clause("Id", z=>z.Equal(1))), 
                    $"UPDATE [Users]{Environment.NewLine}SET [Name] = @p0{Environment.NewLine}WHERE [Id] = @p1", "Update Test 1")
            };
            
            Delete = new[]
            {
                CreateTestCase(x => x.Delete("Users").Where(y=>y.Clause("Id", z=>z.Equal(1))),
                    $"DELETE FROM [Users]{Environment.NewLine}WHERE [Id] = @p0", "Delete Test 1")
            };
        }

        private static TestCaseData CreateTestCase(Action<SqlCommandBuilder> sqlCommandBuilderAction,
            string expectedResult, string testName = default)
        {
            return new TestCaseData(sqlCommandBuilderAction)
            {
                TestName = testName,
                ExpectedResult = expectedResult
            };
        }
    }
}