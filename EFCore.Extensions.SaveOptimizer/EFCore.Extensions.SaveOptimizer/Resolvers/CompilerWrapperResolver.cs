using System.Collections.Concurrent;
using EFCore.Extensions.SaveOptimizer.Wrappers;
using SqlKata.Compilers;

namespace EFCore.Extensions.SaveOptimizer.Resolvers;

public class CompilerWrapperResolver : ICompilerWrapperResolver
{
    private readonly ConcurrentDictionary<string, CompilerWrapper> _compilers;
    public CompilerWrapperResolver() => _compilers = new ConcurrentDictionary<string, CompilerWrapper>();

    public ICompilerWrapper Resolve(string providerName)
    {
        if (_compilers.ContainsKey(providerName))
        {
            return _compilers[providerName];
        }

        Compiler? compiler = null;

        if (providerName.Contains("SqlServer"))
        {
            compiler = new SqlServerCompiler();
        }
        else if (providerName.Contains("Firebird"))
        {
            compiler = new FirebirdCompiler();
        }
        else if (providerName.Contains("MySql"))
        {
            compiler = new MySqlCompiler();
        }
        else if (providerName.Contains("Oracle"))
        {
            compiler = new OracleCompiler();
        }
        else if (providerName.Contains("Postgre"))
        {
            compiler = new PostgresCompiler();
        }
        else if (providerName.Contains("Sqlite"))
        {
            compiler = new SqliteCompiler();
        }

        if (compiler == null)
        {
            throw new ArgumentException("Unexpected provider", nameof(providerName));
        }

        CompilerWrapper wrapper = new(compiler);

        _compilers.TryAdd(providerName, wrapper);

        return wrapper;
    }
}
