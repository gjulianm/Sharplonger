using Hammock;
using System;
using System.Net;

namespace Sharplonger
{
    public class TwitlongerResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public RestRequest Request { get; set; }
        public RestResponse Response { get; set; }
        public string Contents { get; set; }
        public Exception InternalException { get; set; }
        public TwitlongerService Sender { get; set; }
    }
}
