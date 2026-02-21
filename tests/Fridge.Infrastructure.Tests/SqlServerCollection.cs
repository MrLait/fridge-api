using Fridge.Infrastructure.Tests.Helpers;
using Xunit;

namespace Fridge.Infrastructure.Tests;

[CollectionDefinition("SqlServer")]
public sealed class SqlServerCollection : ICollectionFixture<SqlServerContainerFixture>
{
}