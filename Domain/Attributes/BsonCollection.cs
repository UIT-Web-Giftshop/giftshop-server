using System;
using System.Reflection;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }

    public static class BsonCollection
    {
        public static string GetCollectionName<T>() where T : class
        {
            var collectionName = typeof(T).GetCustomAttribute<BsonCollectionAttribute>();
            return collectionName is null ? typeof(T).Name : collectionName.CollectionName;
        }
    }
}