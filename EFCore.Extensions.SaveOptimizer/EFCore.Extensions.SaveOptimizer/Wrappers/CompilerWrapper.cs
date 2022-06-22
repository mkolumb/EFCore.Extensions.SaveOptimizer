using SqlKata;
using SqlKata.Compilers;

namespace EFCore.Extensions.SaveOptimizer.Wrappers
{
    public class CompilerWrapper : ICompilerWrapper
    {
        private readonly Compiler _compiler;

        public CompilerWrapper(Compiler compiler)
        {
            _compiler = compiler;
        }

        public SqlResult Compile(Query query)
        {
            return _compiler.Compile(query);
        }
    }
}
