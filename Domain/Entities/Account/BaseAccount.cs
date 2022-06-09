using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Account
{
    public class BaseAccount
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ImageUrl { get; set; }
        public DateTime LastLogin { get; set; }
    }

    public enum UserRoles
    {
        CLIENT,
        MANAGER,
        ADMIN
    }
}