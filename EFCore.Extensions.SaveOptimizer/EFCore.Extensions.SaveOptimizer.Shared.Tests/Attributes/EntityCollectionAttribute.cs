using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using EFCore.Extensions.SaveOptimizer.Model.Context;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Data;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EntityCollectionAttribute : Attribute
{
    private const string TestMultitaskingDisabledProviders = "TEST_MULTITASKING_DISABLED_PROVIDERS";

    private const string TestMultitaskingProvidersLimit = "TEST_MULTITASKING_PROVIDERS_LIMIT";

    private static readonly ConcurrentDictionary<string, string> CollectionNames = new();

    private readonly Type[] _entityTypes;

    private readonly string _provider;

    private readonly string _realCollectionName;

    public string CollectionName { get; }

    public EntityCollectionAttribute(string provider, params Type[] entityTypes)
    {
        _provider = provider;

        _entityTypes = entityTypes;

        _realCollectionName = ResolveName();

        CollectionName = GetCollectionName();
    }

    private string GetCollectionName()
    {
        if (CollectionNames.ContainsKey(_realCollectionName))
        {
            return CollectionNames[_realCollectionName];
        }

        var prefix = $"EntityCollection_{_provider}";

        var providerLimit = GetProviderLimit();

        string name;

        if (providerLimit.HasValue)
        {
            var index = CollectionNames.Count % providerLimit;

            name = $"{prefix}_{index}";
        }
        else
        {
            name = _realCollectionName;
        }

        CollectionNames[_realCollectionName] = name;

        return name;
    }

    private string ResolveName()
    {
        StringBuilder builder = new(_entityTypes.Length + 1);

        builder.Append("EntityCollection_");

        builder.Append(_provider);

        builder.Append(' ');

        if (MultitaskingSupported())
        {
            builder.AppendJoin("_", _entityTypes.Select(x => x.Name));
        }

        return builder.ToString().Trim().Replace(" ", "_");
    }

    public bool IsConnectedCollection(string collection)
    {
        Type type = typeof(EntitiesContext);

        PropertyInfo? property = type.GetProperty(collection);

        Type argument = property!.PropertyType.GenericTypeArguments[0];

        return _entityTypes.Contains(argument);
    }

    private static bool MultitaskingSupported() => !TestDataHelper.IsDisabled(TestMultitaskingDisabledProviders);

    private static int? GetProviderLimit()
    {
        var value = TestDataHelper.GetProviderValue(TestMultitaskingProvidersLimit);

        return value != null ? int.Parse(value) : null;
    }
}
