using System;

namespace Domain.ViewModels.Profile
{
    public class MyProfileViewModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        
        public string ImageUrl { get; set; }
        
        public string CartId { get; set; }
        
        public string WishlistId { get; set; }
    }
}