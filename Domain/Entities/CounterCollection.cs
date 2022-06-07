using Domain.Attributes;

namespace Domain.Entities
{
    [BsonCollection("counters")]
    public class CounterCollection
    {
        public string CollectionName { get; set; }

        public long CurrentCount { get; set; } 
    }
}