using CommandBuilder.Extensions;
using Dapper;
using NUnit.Framework;
using System;
using System.Linq;

namespace CommandBuilder.Tests
{
    [TestFixture]
    public class Commands_Tests
    {
        public static TestCaseData[] InsertionsTestsSource => CommandBuilderTestCases.Insertions;
        public static TestCaseData[] SelectorsTestsSource => CommandBuilderTestCases.Selectors;
        public static TestCaseData[] UpdatesTestsSource => CommandBuilderTestCases.Updates;
        public static TestCaseData[] DeleteTestsSource => CommandBuilderTestCases.Delete;

        private class TestEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        [TestCaseSource(nameof(InsertionsTestsSource))]
        public string InsertCommand_Correct(Action<SqlCommandBuilder> sqlCommandBuilderAction)
        {
            return RunTestCase(sqlCommandBuilderAction);
        }

        [TestCaseSource(nameof(SelectorsTestsSource))]
        public string SelectCommand_Correct(Action<SqlCommandBuilder> sqlCommandBuilderAction)
        {
            return RunTestCase(sqlCommandBuilderAction);
        }

        [TestCaseSource(nameof(UpdatesTestsSource))]
        public string UpdateCommand_Correct(Action<SqlCommandBuilder> sqlCommandBuilderAction)
        {
            return RunTestCase(sqlCommandBuilderAction);
        }
        
        [TestCaseSource(nameof(DeleteTestsSource))]
        public string DeleteCommand_Correct(Action<SqlCommandBuilder> sqlCommandBuilderAction)
        {
            return RunTestCase(sqlCommandBuilderAction);
        }

        [Test]
        public void InsertCommand_EntityCorrect()
        {
            var expectedSqlScript = $"INSERT INTO [Users]([Id],[Name]){Environment.NewLine}VALUES (@p0, @p1)";

            var testEntity = new TestEntity
            {
                Id = Guid.NewGuid(),
                Name = "TestEntity"
            };

            var dapperCommand = new SqlCommandBuilder()
                .InsertInto("Users", testEntity);

            DynamicParameters parameters = dapperCommand.Parameters as DynamicParameters;

            Assert.AreEqual(expectedSqlScript, dapperCommand.CommandText);
            Assert.NotNull(parameters);
            Assert.AreEqual(2, parameters.ParameterNames.Count());
            Assert.AreEqual(testEntity.Id, parameters.Get<Guid>("p0"));
            Assert.AreEqual(testEntity.Name, parameters.Get<string>("p1"));
        }

        protected string RunTestCase(Action<SqlCommandBuilder> sqlCommandBuilderAction)
        {
            Assert.NotNull(sqlCommandBuilderAction);

            var dapperCommand = new SqlCommandBuilder();

            sqlCommandBuilderAction(dapperCommand);

            return dapperCommand.DapperCommand().CommandText;
        }
    }
}