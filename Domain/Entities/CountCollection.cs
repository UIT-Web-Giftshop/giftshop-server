using Domain.Attributes;

namespace Domain.Entities
{
    [BsonCollection("countCollection")]
    public class CountCollection
    {
        public string CollectionName { get; set; }

        public long CurrentCount { get; set; } 
    }
}