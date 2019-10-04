## Simple fluent SQL Command builder based on DapperORM and DbExtensions/SqlBuilder.cs

Examples:

1. SELECT * FROM [TestTable]
```csharp
new SqlCommandBuilder().Select(x => x.Table(y=>y.Column("*"))).From("TestTable")
```

2. SELECT [tt].[Id] FROM [TestTable] AS [tt]
```csharp
new SqlCommandBuilder().Select(x => x.Table("tt", y=>y.Column("Id"))).From("TestTable", "tt")
```

3. SELECT [a].[Id], [b].[Name] FROM [Login] AS [a] INNER JOIN Users AS [b] ON [a].[Id] = [b].[Id]
```csharp
new SqlCommandBuilder()
    .Select(x => x.Table("a", y => y.Column("Id")).Table("b", y => y.Column("Name")))
    .From("Login", "a")
    .InnerJoin("Users", "b", x =>
    {
        x.LeftSide(y => y.Prefix("a").Column("Id"));
        x.RightSide(y => y.Prefix("b").Column("Id"));
    })
```

4.  SELECT [a].[Id], [b].[Name] FROM [Login] AS [a] INNER JOIN [Users] AS [b] ON [a].[Id] = [b].[Id] WHERE [a].[Id] = @p0
```csharp
new SqlCommandBuilder()
    .Select(y => y
      .Table("[a]", z => z.Column("Id"))
      .Table("b", z => z.Column("[Name]")))
    .From("Login", "[a]")
    .InnerJoin("Users", "[b]", y =>
    {
        y.LeftSide(z => z.Prefix("a").Column("[Id]"));
        y.RightSide(z => z.Prefix("[b]").Column("Id"));
    })
    .Where(y => y.Clause("Id", z => z.Prefix("a").Equal(1))
```

5. INSERT INTO [Users]([Id],[Name]) VALUES (@p0, @p1)
```csharp
new SqlCommandBuilder()
    .InsertInto(x => x.Table("Users").Column("Id").Column("Name"))
    .Values(x => x.Value(1).Value("John")
```

6. INSERT INTO class instance
```csharp
var testEntity = new TestEntity
{
    Id = Guid.NewGuid(),
    Name = "TestEntity"
};

var dapperCommand = new SqlCommandBuilder().InsertInto("Users", testEntity);
```

7. INSERT INTO collection instance
```csharp
var collection = new[] 
{
  new TestEntity
  {
      Id = Guid.NewGuid(),
      Name = "TestEntity"
  }
};

var dapperCommand = new SqlCommandBuilder().InsertInto<TestEntity>("Users", collection);
```

8. UPDATE [Users] SET [Name] = @p0 WHERE [Id] = @p1
```csharp
new SqlCommandBuilder()
    .Update(x => x.Table("Users").Set("Name", "John"))
    .Where(x => x.Clause("Id", y => y.Equal(1))
```

9. DELETE FROM [Users] WHERE [Id] = @p0
```csharp
new SqlCommandBuilder()
    .Delete("Users")
    .Where(x => x.Clause("Id", y => y.Equal(1))
```
