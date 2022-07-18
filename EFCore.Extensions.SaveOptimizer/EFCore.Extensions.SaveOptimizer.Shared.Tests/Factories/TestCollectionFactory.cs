using System.Collections.Concurrent;
using EFCore.Extensions.SaveOptimizer.Shared.Tests.Attributes;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EFCore.Extensions.SaveOptimizer.Shared.Tests.Factories;

// ReSharper disable once UnusedMember.Global
public class TestCollectionFactory : IXunitTestCollectionFactory
{
    private readonly Dictionary<string, ITypeInfo?> _collectionDefinitions;
    private readonly ITestAssembly _testAssembly;
    private readonly ConcurrentDictionary<string, ITestCollection> _testCollections = new();

    public TestCollectionFactory(ITestAssembly testAssembly, IMessageSink diagnosticMessageSink)
    {
        _testAssembly = testAssembly;

        _collectionDefinitions =
            TestCollectionFactoryHelper.GetTestCollectionDefinitions(testAssembly.Assembly, diagnosticMessageSink);
    }

    public string DisplayName => "custom-collection-factory";

    public ITestCollection Get(ITypeInfo testClass)
    {
        string collectionName;

        IAttributeInfo? attributeInfo = testClass
            .GetCustomAttributes(typeof(EntityCollectionAttribute))
            .SingleOrDefault();

        if (attributeInfo == null)
        {
            collectionName = "Test collection for " + testClass.Name;
        }
        else
        {
            collectionName = attributeInfo.GetNamedArgument<string>(nameof(EntityCollectionAttribute.CollectionName));
        }

        return _testCollections.GetOrAdd(collectionName, CreateCollection);
    }

    private ITestCollection CreateCollection(string name)
    {
        _collectionDefinitions.TryGetValue(name, out ITypeInfo? definitionType);

        return new TestCollection(_testAssembly, definitionType, name);
    }
}
