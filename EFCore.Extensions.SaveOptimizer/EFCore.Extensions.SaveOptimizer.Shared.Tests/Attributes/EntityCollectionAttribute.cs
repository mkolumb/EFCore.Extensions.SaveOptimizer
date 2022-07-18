using System.Text;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EntityCollectionAttribute : Attribute
{
    public Type[] EntityTypes { get; }

    public string CollectionName { get; }

    public EntityCollectionAttribute(string provider, params Type[] entityTypes)
    {
        EntityTypes = entityTypes;

        StringBuilder builder = new(entityTypes.Length + 1);

        builder.Append("EntityCollection_");

        builder.Append(provider);

        builder.Append('_');

        builder.AppendJoin("_", entityTypes.Select(x => x.Name));

        CollectionName = builder.ToString();
    }
}
