using System;
using Domain.Attributes;

namespace Domain.Entities.Account
{
    [BsonCollection("users")]
    public class User : BaseAccount
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public bool IsActive { get; set; }
        
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