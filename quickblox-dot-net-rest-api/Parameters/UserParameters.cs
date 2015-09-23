using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;



namespace Kaliido.QuickBlox.Parameters
{
    [JsonObject]
    public class UserParameters
    {
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "old_password")]
        public string OldPassword { get; set; }

        [JsonProperty(PropertyName = "external_user_id")]
        public long? ExternalUserId { get; set; }


        [JsonProperty(PropertyName = "facebook_id")]
        public string FacebookId { get; set; }


        [JsonProperty(PropertyName = "twitter_id")]
        public string TwitterId { get; set; }


        [JsonProperty(PropertyName = "blob_id")]
        public long BlobId { get; set; }


        [JsonProperty(PropertyName = "external_id")]
        public long ExternalId { get; set; }


        [JsonProperty(PropertyName = "website")]
        public string Website { get; set; }


        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }


        [JsonProperty(PropertyName = "tag_list")]
        public string TagList { get; set; }


        [JsonProperty(PropertyName = "custom_data")]
        public IDictionary<string, object> CustomData { get; set; }

        

        public List<string> Tags
        {
            get
            {
                if (TagList != null)
                {
                    return TagList.Split(',').ToList();
                }
                return new List<string>();
            }
            set
            {
                if (value == null || value.Count() == 0)
                {
                    TagList = string.Join(",", value.ToArray());
                }
                else
                {
                    TagList = null;
                }
            }
        }

        









        //user[blob_id]	Optional	Integer	412	ID of associated blob (for example, API User photo)
        //user[phone]	Optional	String	144-556488	Phone
        //user[website]	Optional	String	http://quickblox.com	Website
        //user[custom_data]	Optional	String	I'm at work now	User's additional info
        //user[tag_list]	Optional	String	tag1,tag2,tag3	Tags
        //user[blob_id]	Optional	Integer	412	ID of associated blob (for example, API User photo)
        //user[phone]	Optional	String	144-556488	Phone
        //user[website]	Optional	String	http://quickblox.com	Website
        //user[tag_list]	Optional	String	tag1,tag2,tag3	Tags
        //user[custom_data]	Optional	String	I'm at work now	User's additional information
    }
}
