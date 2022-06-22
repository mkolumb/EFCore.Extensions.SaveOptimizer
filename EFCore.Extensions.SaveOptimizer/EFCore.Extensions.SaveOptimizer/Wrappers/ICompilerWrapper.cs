using SqlKata;

namespace EFCore.Extensions.SaveOptimizer.Wrappers
{
    public interface ICompilerWrapper
    {
        SqlResult Compile(Query query);
    }
}
