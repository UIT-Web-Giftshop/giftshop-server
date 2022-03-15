using System;
using MongoDB.Bson;

namespace Domain.Models
{
    public class RefreshTokenModel
    {
        public string Token { get; set; }
        public string IpAddress { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}