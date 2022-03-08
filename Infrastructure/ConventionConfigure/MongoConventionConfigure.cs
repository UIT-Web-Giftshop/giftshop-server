using MongoDB.Bson.Serialization.Conventions;

namespace Infrastructure.ConventionConfigure
{
    public static class MongoConventionConfigure
    {
        public static void Configure()
        {
            var camelCaseConventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfNullConvention(true)
            };
            ConventionRegistry.Register("CamelCase", camelCaseConventionPack, type => true);
        }
    }
}