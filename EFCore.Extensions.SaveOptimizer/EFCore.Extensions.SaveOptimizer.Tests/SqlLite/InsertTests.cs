using EFCore.Extensions.SaveOptimizer.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Tests.SqlLite;

public class InsertTests : Setup
{
    [Fact]
    public async Task GivenSaveChanges_WhenNoChanges_ShouldDoNothing()
    {
        // Arrange
        EntitiesContext context = ContextResolver();

        // Act
        await context.SaveChangesAsync();

        // Assert
        var counter = await context.NonRelatedEntities.CountAsync();

        counter.Should().Be(0);
    }
}
