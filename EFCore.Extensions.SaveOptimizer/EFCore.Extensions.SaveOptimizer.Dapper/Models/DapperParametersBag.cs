using System.Data;
using System.Data.Common;
using Dapper;
using EFCore.Extensions.SaveOptimizer.Internal.Models;

namespace EFCore.Extensions.SaveOptimizer.Dapper.Models;

public class DapperParametersBag : SqlMapper.IDynamicParameters
{
    private readonly ISqlCommandModel _sql;

    public DapperParametersBag(ISqlCommandModel sql) => _sql = sql;

    public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
    {
        if (_sql.Parameters == null)
        {
            return;
        }

        foreach (SqlParamModel param in _sql.Parameters)
        {
            DbParameter parameter =
                param.SqlValueModel.PropertyTypeModel.ParameterResolver(command, param.Key, param.SqlValueModel.Value);

            command.Parameters.Add(parameter);
        }
    }
}
