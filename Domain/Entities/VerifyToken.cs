using System;
using Domain.Attributes;

namespace Domain.Entities
{
    [BsonCollection("verifyTokens")]
    public class VerifyToken
    {
        public string Email { get; set; }
        
        public string Token { get; set; }
        
        public DateTime Expired { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public bool IsNotExpired()
        {
            return DateTime.Now < Expired;
        }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Token);
        }
    }
}