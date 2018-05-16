using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning.DomainBuilder
{
    using JSONDomain;
    class JSONDomainBuilder
    {
        String filename;
        public JSONDomainBuilder(String filename)
        {
            this.filename = filename;
            create();
        }
        private void create()
        {
            StreamReader r = new StreamReader(filename);
                string json = r.ReadToEnd();
            var jsonDomain = JsonDomain.FromJson(json);
            Console.Write("Deserialized JSON file");
        }
    }

}
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using JSONDomain;
//
//    var jsonDomain = JsonDomain.FromJson(jsonString);

namespace JSONDomain
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class JsonDomain
    {
        [JsonProperty("types")]
        public Instance[] Types { get; set; }

        [JsonProperty("instances")]
        public Instance[] Instances { get; set; }

        [JsonProperty("operators")]
        public Operator[] Operators { get; set; }

        [JsonProperty("initial")]
        public Final Initial { get; set; }

        [JsonProperty("final")]
        public Final Final { get; set; }
    }

    public partial class Final
    {
        [JsonProperty("t")]
        public string[] T { get; set; }

        [JsonProperty("f")]
        public string[] F { get; set; }

        [JsonProperty("characters")]
        public Character[] Characters { get; set; }
    }

    public partial class Character
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bplus")]
        public string[] Bplus { get; set; }

        [JsonProperty("bminus")]
        public string[] Bminus { get; set; }

        [JsonProperty("unsure")]
        public object[] Unsure { get; set; }
    }

    public partial class Instance
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Operator
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("args")]
        public Dictionary<string, string> Args { get; set; }

        [JsonProperty("char")]
        public string Char { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("pre-t")]
        public string[] PreT { get; set; }

        [JsonProperty("pre-f")]
        public string[] PreF { get; set; }

        [JsonProperty("eff-t")]
        public string[] EffT { get; set; }

        [JsonProperty("eff-f")]
        public string[] EffF { get; set; }

        [JsonProperty("pre-bplus")]
        public string[] PreBplus { get; set; }

        [JsonProperty("pre-bminus")]
        public string[] PreBminus { get; set; }

        [JsonProperty("pre-u")]
        public object[] PreU { get; set; }

        [JsonProperty("eff-bplus")]
        public string[] EffBplus { get; set; }

        [JsonProperty("eff-bminus")]
        public string[] EffBminus { get; set; }

        [JsonProperty("eff-u")]
        public object[] EffU { get; set; }
    }

    public partial class JsonDomain
    {
        public static JsonDomain FromJson(string json) => JsonConvert.DeserializeObject<JsonDomain>(json, JSONDomain.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this JsonDomain self) => JsonConvert.SerializeObject(self, JSONDomain.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}

