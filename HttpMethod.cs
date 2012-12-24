using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharplonger
{
    public enum HttpMethod
    {
        Post, Get, Delete, Create, Put
    }

    public static class HttpMethodExtensions
    {
        public static string GetString(this HttpMethod method)
        {
            switch (method)
            {
                case HttpMethod.Create:
                    return "CREATE";
                case HttpMethod.Delete:
                    return "DELETE";
                case HttpMethod.Get:
                    return "GET";
                case HttpMethod.Post:
                    return "POST";
                case HttpMethod.Put:
                    return "PUT";
                default:
                    return "";
            }
        }
    }
}
