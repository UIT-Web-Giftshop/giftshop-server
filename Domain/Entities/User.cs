using System;
using Domain.Attributes;

namespace Domain.Entities
{
    [BsonCollection("users")]
    public class User : IdentifiableObject
    {
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
        //
        // public string CartId { get; set; }
        //
        // public string WishlistId { get; set; }

        public string GetFullName()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}