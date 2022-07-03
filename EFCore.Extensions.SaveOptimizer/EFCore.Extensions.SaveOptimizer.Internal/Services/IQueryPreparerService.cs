﻿using EFCore.Extensions.SaveOptimizer.Internal.Configuration;
using EFCore.Extensions.SaveOptimizer.Internal.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Extensions.SaveOptimizer.Internal.Services;

public interface IQueryPreparerService
{
    void Init(DbContext context);

    IEnumerable<ISqlCommandModel> Prepare(DbContext context, QueryExecutionConfiguration? configuration = null);
}
