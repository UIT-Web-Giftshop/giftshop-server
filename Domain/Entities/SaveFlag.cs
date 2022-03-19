using Domain.Attributes;

namespace Domain.Entities
{
    [BsonCollection("saveFlags")]
    public class SaveFlag
    {
        public string CollectionName { get; set; }

        public long CurrentCount { get; set; } 
    }
}