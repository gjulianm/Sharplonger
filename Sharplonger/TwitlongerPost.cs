using System.Xml.Serialization;

namespace Sharplonger
{
    [XmlRootAttribute("twitlonger")]
    public class TwitlongerPost
    {
        [XmlElement("error")]
        public string Error { get; set; }

        [XmlElement("post")]
        public TLInnerPost Post { get; set; }
    }

    [XmlRootAttribute("post")]
    public class TLInnerPost
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("short")]
        public string ShortenedLink { get; set; }

        [XmlElement("content")]
        public string Content { get; set; }
    }
}
