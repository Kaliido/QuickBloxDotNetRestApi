namespace Kaliido.QuickBlox
{
    public class ApiCredential
    {

        public string ApplicationID { get; set; }
        public string AuthKey { get; set; }
        public string AuthSecret { get; set; }


        public ApiCredential(string applicationID, string authKey, string authSecret)
        {

            this.ApplicationID = applicationID;
            this.AuthKey = authKey;
            this.AuthSecret = authSecret;

        }

    }
}