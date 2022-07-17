using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Enums;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using EFCore.Extensions.SaveOptimizer.Model;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Enums;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Extensions;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;

public abstract partial class BaseMiscTests
{
    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public async Task GivenSaveChangesAsync_WhenFail_ShouldProperlyStoreAfterSaveAgain(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        FailingEntity[] data = { new() { Some = "xyz1" }, new() { Some = "xyz2" }, new() };

        await db.Context.AddRangeAsync(data as IEnumerable<object>).ConfigureAwait(false);

        // Act

        try
        {
            await db.SaveAsync(variant, null, 0).ConfigureAwait(false);
        }
        catch
        {
            data[2].Some = "xyz3";

            await db.SaveAsync(variant, null, 0).ConfigureAwait(false);
        }

        FailingEntity[] result = await db.Context.FailingEntities.OrderBy(x => x.Id).ToArrayWithRetryAsync()
            .ConfigureAwait(false);

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(3);

        keys.Should().AllSatisfy(id => id.Should().BeGreaterThan(0));

        properties.Should().ContainInOrder("xyz1", "xyz2", "xyz3");
    }

    [SkippableTheory]
    [MemberData(nameof(TransactionTestTheoryData))]
    public async Task GivenSaveChangesAsync_WhenTransactionFail_ShouldProperlyStoreAfterSaveAgain(SaveVariant variant,
        AfterSaveBehavior behavior)
    {
        async Task Test()
        {
            // Arrange
            DbContextWrapper[] dbs =
            {
                ContextWrapperResolver(), ContextWrapperResolver(), ContextWrapperResolver(),
                ContextWrapperResolver()
            };

            var connection = dbs[0].Context.Database.GetConnectionString();

            QueryExecutionConfiguration config = new() { AfterSaveBehavior = AfterSaveBehavior.DoNothing };

            dbs[0].CleanDb();

            foreach (DbContextWrapper db in dbs)
            {
                db.RecreateContext(connection);
            }

            await dbs[0].Context.AddRangeAsync(new FailingEntity { Some = "0-xyz" }).ConfigureAwait(false);

            await dbs[0].SaveChangesAsync(variant | SaveVariant.WithTransaction, config).ConfigureAwait(false);

            dbs[0].Dispose();

            var fails = 0;

            (await dbs[1].Context.FailingEntities.FirstAsync().ConfigureAwait(false)).Some = "0-xyz-1";

            FailingEntity[] data1 = { new() { Some = "1-xyz" } };

            await dbs[1].Context.AddRangeAsync(data1 as IEnumerable<object>).ConfigureAwait(false);

            (await dbs[2].Context.FailingEntities.FirstAsync().ConfigureAwait(false)).Some = "0-xyz-2";

            FailingEntity[] data2 =
            {
                new() { Some = "2-xyz-1" }, new() { Some = "2-xyz-2" }, new() { Some = "2-xyz-3" }
            };

            await dbs[2].Context.AddRangeAsync(data2 as IEnumerable<object>).ConfigureAwait(false);

            // Act

            IDbContextTransaction? t1 = null;

            IDbContextTransaction? t2 = null;

            List<Func<Task>> tasks = new()
            {
                async () =>
                {
                    t1 = await dbs[1].Context.Database.BeginTransactionAsync().ConfigureAwait(false);

                    IExecutionResultModel executionResult = await dbs[1]
                        .SaveChangesAsync(variant, config)
                        .ConfigureAwait(false);

                    await t1.CommitAsync().ConfigureAwait(false);

                    executionResult.ProcessAfterSave(behavior);

                    await t1.DisposeAsync().ConfigureAwait(false);

                    t1 = null;
                },
                async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

                    t2 = await dbs[2].Context.Database.BeginTransactionAsync().ConfigureAwait(false);

                    await dbs[2]
                        .SaveChangesAsync(variant, config)
                        .ConfigureAwait(false);

                    throw new Exception("This is controlled exception");
                }
            };

            try
            {
                await Task.WhenAll(tasks.Select(x => x())).ConfigureAwait(false);
            }
            catch
            {
                fails++;

                if (t1 != null)
                {
                    await t1.RollbackAsync().ConfigureAwait(false);

                    await t1.DisposeAsync().ConfigureAwait(false);

                    t1 = null;
                }

                if (t2 != null)
                {
                    await t2.RollbackAsync().ConfigureAwait(false);

                    await t2.DisposeAsync().ConfigureAwait(false);

                    t2 = null;
                }
            }

            IDbContextTransaction t3 = await dbs[2].Context.Database.BeginTransactionAsync().ConfigureAwait(false);

            IExecutionResultModel executionResult = await dbs[2]
                .SaveChangesAsync(variant, config)
                .ConfigureAwait(false);

            await t3.CommitAsync().ConfigureAwait(false);

            executionResult.ProcessAfterSave(behavior);

            FailingEntity[] result = await dbs[3].Context.FailingEntities.OrderBy(x => x.Id).ToArrayWithRetryAsync()
                .ConfigureAwait(false);

            var keys = result.Select(x => x.Id).ToArray();

            var properties = result.Select(x => x.Some).ToArray();

            // Assert
            result.Should().HaveCount(5);

            keys.Should().AllSatisfy(id => id.Should().BeGreaterThan(0));

            properties.Should().ContainInOrder("0-xyz-2", "1-xyz", "2-xyz-1", "2-xyz-2", "2-xyz-3");

            fails.Should().Be(1);
        }

        // Try to run up to 5 times whole test as sometimes transaction can fail randomly
        await RunAsync(5, Test).ConfigureAwait(false);
    }

    [SkippableTheory]
    [MemberData(nameof(BaseWriteTheoryData))]
    public void GivenSaveChanges_WhenFail_ShouldProperlyStoreAfterSaveAgain(SaveVariant variant)
    {
        // Arrange
        using DbContextWrapper db = ContextWrapperResolver();

        FailingEntity[] data = { new() { Some = "xyz1" }, new() { Some = "xyz2" }, new() };

        db.Context.AddRange(data as IEnumerable<object>);

        // Act

        try
        {
            db.Save(variant, null, 0);
        }
        catch
        {
            data[2].Some = "xyz3";

            db.Save(variant, null, 0);
        }

        FailingEntity[] result = db.Context.FailingEntities.OrderBy(x => x.Id).ToArrayWithRetry();

        var keys = result.Select(x => x.Id).ToArray();

        var properties = result.Select(x => x.Some).ToArray();

        // Assert
        result.Should().HaveCount(3);

        keys.Should().AllSatisfy(id => id.Should().BeGreaterThan(0));

        properties.Should().ContainInOrder("xyz1", "xyz2", "xyz3");
    }

    [SkippableTheory]
    [MemberData(nameof(TransactionTestTheoryData))]
    public void GivenSaveChanges_WhenTransactionFail_ShouldProperlyStoreAfterSaveAgain(SaveVariant variant,
        AfterSaveBehavior behavior)
    {
        void Test()
        {
            // Arrange
            DbContextWrapper[] dbs =
            {
                ContextWrapperResolver(), ContextWrapperResolver(), ContextWrapperResolver(),
                ContextWrapperResolver()
            };

            var connection = dbs[0].Context.Database.GetConnectionString();

            QueryExecutionConfiguration config = new() { AfterSaveBehavior = AfterSaveBehavior.DoNothing };

            dbs[0].CleanDb();

            foreach (DbContextWrapper db in dbs)
            {
                db.RecreateContext(connection);
            }

            dbs[0].Context.AddRange(new FailingEntity { Some = "0-xyz" });

            dbs[0].SaveChanges(variant | SaveVariant.WithTransaction, config);

            dbs[0].Dispose();

            var fails = 0;

            dbs[1].Context.FailingEntities.First().Some = "0-xyz-1";

            FailingEntity[] data1 = { new() { Some = "1-xyz" } };

            dbs[1].Context.AddRange(data1 as IEnumerable<object>);

            dbs[2].Context.FailingEntities.First().Some = "0-xyz-2";

            FailingEntity[] data2 =
            {
                new() { Some = "2-xyz-1" }, new() { Some = "2-xyz-2" }, new() { Some = "2-xyz-3" }
            };

            dbs[2].Context.AddRange(data2 as IEnumerable<object>);

            // Act

            IDbContextTransaction? t1 = null;

            IDbContextTransaction? t2 = null;

            List<Action> tasks = new()
            {
                () =>
                {
                    t1 = dbs[1].Context.Database.BeginTransaction();

                    IExecutionResultModel executionResult = dbs[1].SaveChanges(variant, config);

                    t1.Commit();

                    executionResult.ProcessAfterSave(behavior);

                    t1.Dispose();

                    t1 = null;
                },
                () =>
                {
                    Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false).GetAwaiter().GetResult();

                    t2 = dbs[2].Context.Database.BeginTransaction();

                    dbs[2].SaveChanges(variant, config);

                    throw new Exception("This is controlled exception");
                }
            };

            try
            {
                tasks.ForEach(x => x());
            }
            catch
            {
                fails++;

                if (t1 != null)
                {
                    t1.Rollback();

                    t1.Dispose();

                    t1 = null;
                }

                if (t2 != null)
                {
                    t2.Rollback();

                    t2.Dispose();

                    t2 = null;
                }
            }

            IDbContextTransaction t3 = dbs[2].Context.Database.BeginTransaction();

            IExecutionResultModel executionResult = dbs[2].SaveChanges(variant, config);

            t3.Commit();

            executionResult.ProcessAfterSave(behavior);

            FailingEntity[] result = dbs[3].Context.FailingEntities.OrderBy(x => x.Id).ToArrayWithRetry();

            var keys = result.Select(x => x.Id).ToArray();

            var properties = result.Select(x => x.Some).ToArray();

            // Assert
            result.Should().HaveCount(5);

            keys.Should().AllSatisfy(id => id.Should().BeGreaterThan(0));

            properties.Should().ContainInOrder("0-xyz-2", "1-xyz", "2-xyz-1", "2-xyz-2", "2-xyz-3");

            fails.Should().Be(1);
        }

        // Try to run up to 5 times whole test as sometimes transaction can fail randomly
        Run(5, Test);
    }
}
