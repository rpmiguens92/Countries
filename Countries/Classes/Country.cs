using Newtonsoft.Json;
 

namespace Countries.Classes
{
    public class Country
    {

        //[JsonProperty("name")]
        //public Name Names { get; set; }

        //[JsonProperty("capital")]
        //public List<string> Capital { get; set; }

        //[JsonProperty("region")]
        //public string Region { get; set; }

        //[JsonProperty("subregion")]
        //public string Subregion { get; set; }

        //[JsonProperty("population")]
        //public int Population { get; set; }

        //[JsonProperty("currencies")]
        //public string Currency { get; set; }

        //[JsonProperty("gini")]
        //public Dictionary<string, float> Gini { get; set; }

        //[JsonProperty("flags")]
        //public Flag Flags { get; set; }

        //[JsonProperty("maps")]
        //public Map Maps { get; set; }

        [JsonProperty("name")]
        public Name Nome { get; set; }

        [JsonProperty("capital")]
        public List<string> Capital { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("subregion")]
        public string Subregion { get; set; }

        [JsonProperty("population")]
        public int Population { get; set; }

        [JsonProperty("gini")]
        public Dictionary<string, float> Gini { get; set; }

        [JsonProperty("flags")]
        public Flag Flags { get; set; }

    }
}

