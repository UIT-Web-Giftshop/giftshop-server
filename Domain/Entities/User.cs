using System;
using Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    [BsonCollection("users")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        
        public string CartId { get; set; }
        
        public string WishlistId { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}