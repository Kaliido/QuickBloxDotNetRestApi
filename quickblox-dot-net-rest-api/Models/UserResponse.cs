using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Kaliido.QuickBlox.Models
{
    public class QuickbloxUser
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Phone { get; set; }
        public object Website { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastRequestAt { get; set; }
        public object ExternalUserId { get; set; }
        public string FacebookId { get; set; }
        public object TwitterId { get; set; }
        public int? BlobId { get; set; }
        public string CustomData { get; set; }
        public string UserTags { get; set; }

        public List<string> Tags { get { if (UserTags != null) { return UserTags.Split(',').ToList(); } else { return new List<string>(); } } }

        public Dictionary<string, object> Custom { get { return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(CustomData); } }
    }
}