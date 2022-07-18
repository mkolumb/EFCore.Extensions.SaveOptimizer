﻿using EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Wrappers;
using Xunit.Abstractions;

namespace EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Tests;

public static class WrapperResolver
{
    public static DbContextWrapper ContextWrapperResolver(ITestOutputHelper testOutputHelper, EntityCollectionAttribute? collectionAttribute)
    {
        PomeloMariaDbDesignTimeFactory factory = new();

        const string query = "truncate `{0}`;";

        DbContextWrapper wrapper = new(factory, testOutputHelper, query);

        wrapper.Migrate();

        wrapper.CleanDb(collectionAttribute);

        return wrapper;
    }
}
