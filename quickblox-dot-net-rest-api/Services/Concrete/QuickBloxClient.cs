using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Kaliido.QuickBlox.Models;
using Kaliido.QuickBlox.Parameters;
using Kaliido.QuickBlox.Responses;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;
using System.Data;
using Kaliido.QuickBloxDotNet.Api.Enums;
using System.Collections.Generic;

namespace Kaliido.QuickBlox
{
    public class QuickBloxClient : IQuickBloxClient
    {

        public string BaseUrl = "https://api.quickblox.com";
        public ApiCredential Credentials { get; set; }
        public UserCredential UserCredentials { get; set; }
        public IRestClient Client { get; set; }
        public string QBToken { get; set; }


        public QuickBloxClient(ApiCredential credentials, UserCredential adminUserCredential, string baseUrl = "https://api.quickblox.com")
        {
            Credentials = credentials;
            UserCredentials = adminUserCredential;
            Client = new RestSharp.RestClient(baseUrl);
        }



        public QuickBloxClient(ApiCredential credentials, string baseUrl = "https://api.quickblox.com")
        {
            Credentials = credentials;
            Client = new RestSharp.RestClient(baseUrl);
        }

        public QuickBloxClient()
        {
       
            Client = new RestSharp.RestClient(BaseUrl);
        }

        public async Task<dynamic> Login(UserCredential credentials)
        {
            UserCredentials = credentials;
            return await GetQBToken();
        }

        public async Task<dynamic> Login(string userName, string password)
        {
            UserCredentials = new UserCredential() { UserLogin = userName, Password = password };
            return await GetQBToken();
        }

        public void ValidateBeforeRequest(RequestValidationType tokenType)
        {

            if (this.Credentials == null)
            {
                throw new ArgumentNullException("You must set the Quickblox API Credentials Object to the QuickBloxClient.");
            }
            else
            {


                if (String.IsNullOrEmpty(this.Credentials.ApplicationID) || String.IsNullOrEmpty(this.Credentials.AuthKey) || String.IsNullOrEmpty(this.Credentials.AuthSecret))
                {
                    throw new ArgumentNullException("The Application Credentials must be set in the Quick Blox Client, you require ApplicationID, AuthKey & AuthSecret");
                }

            }

            if (tokenType == RequestValidationType.QuickBloxUserToken)
            {

                if (this.UserCredentials == null || String.IsNullOrEmpty(UserCredentials.UserLogin) || String.IsNullOrEmpty(UserCredentials.Password))
                {
                    throw new ArgumentNullException("The request is an quickblox user orientated request.\n This kind of request requires you to enter login credentials as either the Admin user, or a user of the quickblox app you are trying to use.");
                }
            }
        }

        public async Task<string> GetQBToken()
        {



            var nonce = GlobalHelper.getNonce();
            var timeStamp = GlobalHelper.getTimestamp();

            var request = new RestRequest(Method.POST);
            request.Resource = "session.json";
            request.AddParameter("application_id", Credentials.ApplicationID);
            request.AddParameter("auth_key", Credentials.AuthKey);
            request.AddParameter("nonce", nonce);
            request.AddParameter("timestamp", timeStamp);



            var postData = new StringBuilder();
            postData.AppendFormat("application_id={0}", Credentials.ApplicationID);
            postData.AppendFormat("&auth_key={0}", Credentials.AuthKey);
            postData.AppendFormat("&nonce={0}", nonce);
            postData.AppendFormat("&timestamp={0}", timeStamp);
            postData.AppendFormat("&user[login]={0}", UserCredentials.UserLogin);
            postData.AppendFormat("&user[password]={0}", UserCredentials.Password);


            var signature = GlobalHelper.getHash(postData.ToString(), Credentials.AuthSecret).ByteToString();


            request.AddParameter("signature", signature);

            request.AddParameter("user[login]", UserCredentials.UserLogin);
            request.AddParameter("user[password]", UserCredentials.Password);
            request.AddHeader("QuickBlox-REST-API-Version", "0.1.0");
            request.RequestFormat = RestSharp.DataFormat.Json;

            var a = await Client.ExecuteTaskAsync<Token>(request);

            if (a.ResponseStatus == ResponseStatus.Completed && a.ErrorMessage == null)
            {
                return a.Data.Session.Token;
            }
            else
            {
                throw a.ErrorException;
            }
        }

        public async Task<QuickbloxUser> RegisterAsync(UserParameters userParameters)
        {
            var request = new RestRequest("users.json", Method.POST)
                          {
                              RequestFormat = DataFormat.Json,
                              JsonSerializer = new NewtonsoftSerializer()
                          };
            request.AddJsonBody(new { user = userParameters });

            await PrepareRequestForQuickblox(request);

            var result = await Client.ExecutePostTaskAsync<UserResponse>(request);

            if (result.ResponseStatus != ResponseStatus.Completed)
            {
                return null;
            }
            if (result.StatusCode == HttpStatusCode.Created)
                return result.Data.User;

            return null;
        }

        public async Task<string> GetApplicationToken()
        {
            ValidateBeforeRequest(RequestValidationType.QuickBlockApplicationToken);
            var nonce = GlobalHelper.getNonce();
            var timeStamp = GlobalHelper.getTimestamp();

            var request = new RestRequest(Method.POST);
            request.Resource = "session.json";
            request.AddParameter("application_id", Credentials.ApplicationID);
            request.AddParameter("auth_key", Credentials.AuthKey);
            request.AddParameter("nonce", nonce);
            request.AddParameter("timestamp", timeStamp);



            var postData = new StringBuilder();
            postData.AppendFormat("application_id={0}", Credentials.ApplicationID);
            postData.AppendFormat("&auth_key={0}", Credentials.AuthKey);
            postData.AppendFormat("&nonce={0}", nonce);
            postData.AppendFormat("&timestamp={0}", timeStamp);


            string signature = GlobalHelper.getHash(postData.ToString(), Credentials.AuthSecret).ByteToString();


            request.AddParameter("signature", signature);

            request.AddHeader("QuickBlox-REST-API-Version", "0.1.0");
            request.RequestFormat = DataFormat.Json;

            var a = await Client.ExecuteTaskAsync<Token>(request);

            if (a.ResponseStatus == ResponseStatus.Completed && a.StatusCode == HttpStatusCode.Created)
            {
                return a.Data.Session.Token;
            }
            return null;
        }

        public async Task<QuickbloxUser> RegisterByExternal(string provider, string scope, string token)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "login.json";

            request.AddParameter("provider", provider.ToLower());
            if (provider.ToLower() == "facebook")
            {
                request.AddParameter("user[login]", "");
                request.AddParameter("scope", "email");
                request.AddParameter("keys[token]", token);
            }
            if (provider.ToLower() == "twitter")
            {
                request.AddParameter("keys[secret]", token);
            }

            request.AddHeader("QuickBlox-REST-API-Version", "0.1.0");

            var qbToken = await GetApplicationToken();
            request.AddHeader("QB-Token", qbToken);

            request.RequestFormat = RestSharp.DataFormat.Json;

            var result = await Client.ExecuteTaskAsync<UserResponse>(request);

            if (result.ResponseStatus != ResponseStatus.Completed)
                return null;

            if (result.StatusCode == HttpStatusCode.Accepted || result.StatusCode == HttpStatusCode.Created)
            {
                return result.Data.User;
            }
            return null;
        }

        public async Task<QuickbloxUser> UpdateUserAsync(long userId, UserParameters userParameters)
        {
            var request = new RestRequest(String.Format("users/{0}.json", userId), Method.PUT);

            await PrepareRequestForQuickblox(request);

            request.AddJsonBody(new { user = userParameters });



            var result = await Client.ExecuteTaskAsync<UserResponse>(request);

            if (result.ResponseStatus == ResponseStatus.Completed && result.Data.User.Id == userId)
                return result.Data.User;

            return null;
        }
        



        public async Task<QuickbloxUser> RemoveExternalLogin(long userId, UserParameters userParameters)
        {
            var request = new RestRequest(String.Format("users/{0}.json", userId), Method.PUT);

            await PrepareRequestForQuickblox(request);
            request.JsonSerializer = new NewtonsoftSerializer(new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include });

            if (userParameters.FacebookId == "0")
            {
                request.AddJsonBody(new { user = new FacebookParameter() });
            }

            var result = await Client.ExecuteTaskAsync<UserResponse>(request);

            if (result.ResponseStatus == ResponseStatus.Completed && result.Data.User.Id == userId)
                return result.Data.User;

            return null;

        }

        private async Task PrepareRequestForQuickblox(RestRequest request)
        {
            ValidateBeforeRequest(RequestValidationType.QuickBloxUserToken);
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new NewtonsoftSerializer();
            request.AddHeader("QuickBlox-REST-API-Version", "0.1.0");
            string token = await GetQBToken();
            request.AddHeader("QB-Token", token);
        }

        public async Task<dynamic> GetUsersClosest(double lat, double longitude)
        {
            var request = new RestRequest("/geodata/find.json", Method.GET);

            request.AddQueryParameter("last_only", "1");
            request.AddQueryParameter("radius", "3000");
            request.AddQueryParameter("current_position", String.Format("{0};{1}", lat, longitude));
            request.AddQueryParameter("sort_by", "distance");
            request.AddQueryParameter("sort_asc", "1");

            await PrepareRequestForQuickblox(request);
            var response = await Client.ExecuteTaskAsync(request);

            dynamic userList = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);


            //dynamic responseList = userList.ToList().Select(a => new
            //                                                       {
            //                                                           Rank = userList.ToList().IndexOf(a),
            //                                                           a.User.Id,
            //                                                           a.User.FullName,
            //                                                           a.User.Email,
            //                                                           a.User.CustomData
            //                                                       }).ToList();

            return userList;
        }

        public async Task<UsersResponse> GetAllUsers()
        {
            var request = new RestRequest("users.json", Method.GET);
            
            request.AddParameter("page", 1);
            request.AddParameter("per_page", 50);

            await PrepareRequestForQuickblox(request);
            var result = await Client.ExecuteTaskAsync<UsersResponse>(request);

            if (result.ResponseStatus == ResponseStatus.Completed)
                return result.Data;

            return null;
        }

        public async Task<QuickbloxUser> GetUser(string userLogin)
        {
            var request = new RestRequest(String.Format("users/by_login.json?login={0}", userLogin), Method.GET);

            await PrepareRequestForQuickblox(request);

            var result = await Client.ExecuteTaskAsync<UserResponse>(request);

            if (result.ResponseStatus == ResponseStatus.Completed && result.Data.User.Login == userLogin)
                return result.Data.User;

            return null;
        }

        public async Task<QuickbloxUser> GetUserById(long id)
        {
            var request = new RestRequest(String.Format("users/{0}.json", id.ToString()), Method.GET);

            await PrepareRequestForQuickblox(request);

            var result = await Client.ExecuteTaskAsync<UserResponse>(request);

            if (result.ResponseStatus == ResponseStatus.Completed && result.Data.User.Id == id)
                return result.Data.User;

            return null;
        }

        public async Task<QuickbloxUser> GetUserByFullName(string userFullName)
        {
            var request = new RestRequest(String.Format("users/by_full_name.json?full_name={0}", userFullName), Method.GET);

            await PrepareRequestForQuickblox(request);

            var result = await Client.ExecuteTaskAsync<UserResponse>(request);

            if (result.ResponseStatus == ResponseStatus.Completed && result.Data.User.FullName == userFullName)
                return result.Data.User;

            return null;
        }

        public async Task<UsersResponse> GetUsersByTags(string tags)
        {
            var request = new RestRequest(String.Format("users/by_tags.json?tags={0}", tags), Method.GET);

            await PrepareRequestForQuickblox(request);

            var result = await Client.ExecuteTaskAsync<UsersResponse>(request);

            if (result.ResponseStatus == ResponseStatus.Completed)
                return result.Data;

            return null;
        }


    }
}