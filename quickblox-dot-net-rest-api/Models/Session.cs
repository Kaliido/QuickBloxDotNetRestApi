using System;

namespace Kaliido.QuickBlox.Models
{
    internal class Session
    {
        public int ApplicationId { get; set; }
        public DateTime CreatedAt { get; set; }

        public string DeviceId { get; set; }
        public int Id { get; set; }
        public int Nonce { get; set; }
        public string Token { get; set; }
        public long Ts { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UserId { get; set; }
    }
}