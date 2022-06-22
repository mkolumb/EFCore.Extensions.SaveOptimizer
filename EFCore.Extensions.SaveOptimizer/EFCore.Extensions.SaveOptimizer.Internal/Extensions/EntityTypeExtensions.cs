using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Extensions;

public static class EntityTypeExtensions
{
    private static readonly ConcurrentDictionary<string, IDictionary<Type, int>> Cache = new();

    public static IDictionary<Type, int> ResolveEntityHierarchy(this IModel model)
    {
        var name = model.ToDebugString();

        if (!Cache.ContainsKey(name))
        {
            Cache[name] = model.GetEntityTypes().ResolveEntityHierarchy(_ => true);
        }

        return Cache[name];
    }

    public static IDictionary<Type, int> ResolveEntityHierarchy(this IModel model, HashSet<Type> usedTypes) =>
        model.GetEntityTypes().ResolveEntityHierarchy(x => usedTypes.Contains(x.ClrType));

    public static IDictionary<Type, int> ResolveEntityHierarchy(this IEnumerable<IEntityType> entities) =>
        entities.ResolveEntityHierarchy(_ => true);

    private static IDictionary<Type, int> ResolveEntityHierarchy(this IEnumerable<IEntityType> entities,
        Func<IEntityType, bool> necessary)
    {
        Dictionary<int, IEntityType> hierarchy = new();

        var index = 0;

        HashSet<string> builder = new();

        foreach (IEntityType entityType in entities)
        {
            if (!necessary(entityType))
            {
                continue;
            }

            builder.Add(entityType.GetSchemaQualifiedTableName() ?? string.Empty);

            hierarchy.Add(index, entityType);

            index++;
        }

        var name = string.Join("|", builder);

        if (!Cache.ContainsKey(name))
        {
            ShiftEntities(hierarchy);

            Dictionary<Type, int> value = hierarchy.ToDictionary(x => x.Value.ClrType, x => x.Key);

            Cache[name] = value;
        }

        return Cache[name];
    }

    private static void ShiftEntities(IDictionary<int, IEntityType> hierarchy)
    {
        while (true)
        {
            var changed = false;

            for (var i = 0; i < hierarchy.Count; i++)
            {
                for (var j = i + 1; j < hierarchy.Count; j++)
                {
                    IEntityType firstEntity = hierarchy[i];

                    IEntityType secondEntity = hierarchy[j];

                    if (!ShouldSwitch(firstEntity, secondEntity))
                    {
                        continue;
                    }

                    hierarchy[i] = secondEntity;
                    hierarchy[j] = firstEntity;
                    changed = true;
                    break;
                }
            }

            if (changed)
            {
                continue;
            }

            break;
        }
    }

    private static bool ShouldSwitch(IEntityType firstEntity, IEntityType secondEntity)
    {
        IEnumerable<IForeignKey> keys = firstEntity.GetForeignKeys();

        return keys.Any(x => x.PrincipalEntityType.Name == secondEntity.Name);
    }
}
