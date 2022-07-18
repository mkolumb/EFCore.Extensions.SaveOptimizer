﻿using EFCore.Extensions.SaveOptimizer.Model.Entities;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Tests;
using Xunit.Abstractions;

// ReSharper disable UnusedMember.Global

namespace EFCore.Extensions.SaveOptimizer.Oracle.Tests.Tests;

[EntityCollection(Variables.ProviderName, typeof(ComposedPrimaryKeyEntity))]
public class ComposedPrimaryKeyTests : BaseComposedPrimaryKeyTests
{
    public ComposedPrimaryKeyTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper, WrapperResolver.ContextWrapperResolver)
    {
    }
}
