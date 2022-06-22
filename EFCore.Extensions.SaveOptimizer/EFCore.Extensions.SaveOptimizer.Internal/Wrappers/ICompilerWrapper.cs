﻿using SqlKata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Wrappers
{
    public interface ICompilerWrapper
    {
        SqlResult Compile(Query query);
    }
}