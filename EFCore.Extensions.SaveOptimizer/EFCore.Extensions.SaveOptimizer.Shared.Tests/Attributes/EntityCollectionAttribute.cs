using System.Reflection;
using System.Text;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EntityCollectionAttribute : Attribute
{
    private const string TestMultitaskingDisabledProviders = "TEST_MULTITASKING_DISABLED_PROVIDERS";

    private readonly Type[] _entityTypes;

    public string CollectionName { get; }

    public EntityCollectionAttribute(string provider, params Type[] entityTypes)
    {
        _entityTypes = entityTypes;

        StringBuilder builder = new(entityTypes.Length + 1);

        builder.Append("EntityCollection_");

        builder.Append(provider);

        builder.Append(' ');

        if (MultitaskingSupported())
        {
            builder.AppendJoin("_", entityTypes.Select(x => x.Name));
        }

        CollectionName = builder.ToString().Trim().Replace(" ", "_");
    }

    public bool IsConnectedCollection(string collection)
    {
        Type type = typeof(EntitiesContext);

        PropertyInfo? property = type.GetProperty(collection);

        Type argument = property!.PropertyType.GenericTypeArguments[0];

        return _entityTypes.Contains(argument);
    }

    private static bool MultitaskingSupported() =>
        !TestDataHelper.IsDisabled(TestDataHelper.GetValues(TestMultitaskingDisabledProviders));
}
