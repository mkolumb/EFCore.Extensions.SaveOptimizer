namespace EFCore.Extensions.SaveOptimizer.Internal.Wrappers;

public class CompilerWrapper : ICompilerWrapper
{
    private readonly Compiler _compiler;

    public CompilerWrapper(Compiler compiler)
    {
        _compiler = compiler;

        SetParametersCount();
    }

    public SqlResult Compile(Query query) => _compiler.Compile(query);

    public int MaxParametersCount { get; private set; }

    private void SetParametersCount()
    {
        Type type = _compiler.GetType();

        Dictionary<Type, Action> @switch = new()
        {
            { typeof(SqlServerCompiler), () => MaxParametersCount = 2048 },
            { typeof(PostgresCompiler), () => MaxParametersCount = 31768 },
            { typeof(MySqlCompiler), () => MaxParametersCount = 15384 },
            { typeof(FirebirdCompiler), () => MaxParametersCount = 15384 },
            { typeof(SqliteCompiler), () => MaxParametersCount = 512 },
            { typeof(OracleCompiler), () => MaxParametersCount = 15384 }
        };

        if (@switch.ContainsKey(type))
        {
            @switch[type]();
        }
        else
        {
            MaxParametersCount = 15384;
        }
    }
}
