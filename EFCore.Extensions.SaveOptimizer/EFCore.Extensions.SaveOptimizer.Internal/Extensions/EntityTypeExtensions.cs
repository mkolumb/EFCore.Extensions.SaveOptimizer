using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.Extensions.SaveOptimizer.Internal.Extensions;

public static class EntityTypeExtensions
{
    public static IDictionary<Type, int> ResolveEntityHierarchy(this IEnumerable<IEntityType> entities)
    {
        Dictionary<int, IEntityType> hierarchy = new();

        var index = 0;

        foreach (IEntityType entityType in entities)
        {
            hierarchy.Add(index, entityType);

            index++;
        }

        ShiftEntities(hierarchy);

        return hierarchy.ToDictionary(x => x.Value.ClrType, x => x.Key);
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
