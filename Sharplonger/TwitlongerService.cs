using AncoraMVVM.Rest;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sharplonger
{
    public class TwitlongerService : BaseService
    {
        private string appName;
        private string apiKey;
        private string username;
        XmlSerializer serializer;

        public string Username { get { return username; } }

        public TwitlongerService(string app, string key, string user)
            : base(new HttpService())
        {
            appName = app;
            apiKey = key;
            username = user;

            Authority = "http://www.twitlonger.com/";
            BasePath = "";
            serializer = new XmlSerializer(typeof(TwitlongerPost));

            PersistentUrlParameters.Add("application", appName);
            PersistentUrlParameters.Add("api_key", apiKey);
            PersistentUrlParameters.Add("username", username);
        }

        protected override T Deserialize<T>(string content)
        {
            T deserialized = default(T); ;
            byte[] buffer = Encoding.UTF8.GetBytes(content);

            MemoryStream stream = null;
            using (stream = new MemoryStream(buffer))
            {
                deserialized = (T)serializer.Deserialize(stream);
            }

            return deserialized;
        }

        protected override async Task<string> GetErrorMessage(HttpResponseMessage response)
        {
            var contents = await response.Content.ReadAsStringAsync();
            Regex regex = new Regex("<error>(.*)</error>");

            var match = regex.Match(contents);

            if (match.Success)
                return match.Groups[1].Value;
            else
                return "";
        }

        #region Methods
        public async Task<HttpResponse<TwitlongerPost>> PostUpdate(string message)
        {
            return await CreateAndExecute<TwitlongerPost>("api_post", HttpMethod.Post, "message", message);
        }

        public async Task<HttpResponse<TwitlongerPost>> PostUpdate(string message, long replyId, string replyUser)
        {
            return await CreateAndExecute<TwitlongerPost>("api_post", HttpMethod.Post, "message", message, "in_reply", replyId, "in_reply_user", replyUser);
        }

        public async Task<HttpResponse> SetId(string messageId, long twitterId)
        {
            return await CreateAndExecute("api_set_id", HttpMethod.Post, "message_id", messageId, "twitter_id", twitterId);
        }
        #endregion


    }
}
