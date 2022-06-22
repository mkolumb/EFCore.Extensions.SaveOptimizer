﻿using BenchmarkDotNet.Attributes;
using EFCore.Extensions.SaveOptimizer.Model;

namespace EFCore.Extensions.SaveOptimizer.Shared.Benchmark;

public abstract class BaseInsertBenchmark
{
    private readonly IWrapperResolver _contextResolver;
    private IDbContextWrapper? _context;

    public abstract long Rows { get; set; }

    [Params(SaveVariant.Optimized | SaveVariant.WithTransaction, SaveVariant.EfCore | SaveVariant.WithTransaction)]
    public SaveVariant Variant { get; set; }

    protected BaseInsertBenchmark(IWrapperResolver contextResolver) => _contextResolver = contextResolver;

    [Benchmark(OperationsPerInvoke = 1)]
    public async Task ExecuteAsync()
    {
        if (_context == null)
        {
            throw new ArgumentNullException(nameof(_context));
        }

        for (var i = 0L; i < Rows; i++)
        {
            NonRelatedEntity entity = CreateItem(i);

            await _context.Context.NonRelatedEntities.AddAsync(entity);
        }

        await _context.Save(Variant);
    }

    private static NonRelatedEntity CreateItem(long i) =>
        new()
        {
            ConcurrencyToken = DateTimeOffset.UtcNow,
            SomeNonNullableBooleanProperty = true,
            SomeNonNullableDateTimeProperty = new DateTimeOffset(2010, 10, 10, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNullableDateTimeProperty = new DateTimeOffset(2012, 11, 11, 1, 2, 3, 0, TimeSpan.Zero),
            SomeNonNullableDecimalProperty = 2.52M,
            SomeNullableDecimalProperty = 4.523M,
            SomeNonNullableIntProperty = 1,
            SomeNullableIntProperty = 11,
            SomeNonNullableStringProperty = $"some-string-{i}",
            SomeNullableStringProperty = "other-string"
        };

    [GlobalSetup]
    public async Task Setup()
    {
        _context = _contextResolver.Resolve();

        await _context.Truncate();
    }

    [GlobalCleanup]
    public void Cleanup() => _context?.Dispose();
}