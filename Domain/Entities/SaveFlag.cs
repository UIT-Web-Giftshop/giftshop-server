using System;
using Domain.Attributes;

namespace Domain.Entities
{
    [BsonCollection("save-flags")]
    public class SaveFlag
    {
        public string CollectionName { get; set; }
        public long CurrentCount { get; set; } 
    }
}