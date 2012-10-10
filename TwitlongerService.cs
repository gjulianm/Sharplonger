using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Hammock.Web;
using Hammock;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Sharplonger
{
    public class TwitlongerService
    {
        private string appName;
        private string apiKey;
        private string username;
        RestClient client;
        XmlSerializer serializer;

        public string Username { get { return username; } }

        public TwitlongerService(string app, string key, string user)
        {
            appName = app;
            apiKey = key;
            username = user;

            client = new RestClient();
            client.Authority = "http://www.twitlonger.com/";
            serializer = new XmlSerializer(typeof(TwitlongerPost));
        }

        void WithHammock<T>(string url, WebMethod method, WebParameterCollection parameters, Action<T, TwitlongerResponse> callback)
        {
            RestRequest request = new RestRequest();
            request.Path = url;
            request.Method = method;

            foreach (var parameter in parameters)
                request.AddParameter(parameter.Name, parameter.Value);

            AddAuthentication(request);

            client.BeginRequest(request, ClientCallback<T>, callback);
        }

        void AddAuthentication(RestRequest request)
        {
            request.AddParameter("application", appName);
            request.AddParameter("api_key", apiKey);
            request.AddParameter("username", username);
        }

        void ClientCallback<T>(RestRequest request, RestResponse response, object userState)
        {
            Action<T, TwitlongerResponse> callback = userState as Action<T, TwitlongerResponse>;
            T deserialized;

            TwitlongerResponse tlResponse = new TwitlongerResponse 
            {
                Contents = response.Content,
                Request = request,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription,
                Response = response,
                Sender = this
            };

            if(typeof(T) == typeof(string))
                deserialized = (T) (object) response.Content;
            else
                deserialized = DeserializeObject<T>(response, tlResponse);

            callback(deserialized, tlResponse);
        }

        private T DeserializeObject<T>(RestResponse response, TwitlongerResponse tlResponse)
        {
            T deserialized = default(T);;
            byte[] buffer = Encoding.UTF8.GetBytes(response.Content);

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(buffer);
                deserialized = (T)serializer.Deserialize(stream);
            }
            catch (Exception e)
            {
                tlResponse.InternalException = e;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return deserialized;
        }

        #region Methods
        public void PostUpdate(string message, Action<TwitlongerPost, TwitlongerResponse> callback)
        {
            WebParameterCollection collection = new WebParameterCollection();
            collection.Add(new WebPair("message", message));

            WithHammock<TwitlongerPost>("api_post", WebMethod.Post, collection, callback);
        }

        public void PostUpdate(string message, long replyId, string replyUser, Action<TwitlongerPost, TwitlongerResponse> callback)
        {
            WebParameterCollection collection = new WebParameterCollection();
            collection.Add(new WebPair("message", message));
            collection.Add(new WebPair("in_reply", replyId.ToString()));
            collection.Add(new WebPair("in_reply_user", replyUser));

            WithHammock<TwitlongerPost>("api_post", WebMethod.Post, collection, callback);
        }

        public void SetId(string messageId, long twitterId, Action<string, TwitlongerResponse> callback)
        {
            WebParameterCollection collection = new WebParameterCollection();
            collection.Add(new WebPair("message_id", messageId));
            collection.Add(new WebPair("twitter_id", twitterId.ToString()));

            WithHammock<string>("api_set_id", WebMethod.Post, collection, callback);
        }
        #endregion
    }
}
