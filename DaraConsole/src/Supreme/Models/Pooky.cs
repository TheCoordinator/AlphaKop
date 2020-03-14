using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DaraBot.Supreme.Models
{
    public partial struct Pooky
    {
        [JsonProperty("cookies")]
        public Cookies Cookies { get; internal set; }

        [JsonProperty("pageData")]
        public PageData PageData { get; internal set; }

        [JsonProperty("hostname")]
        public string Hostname { get; internal set; }

        [JsonConstructor]
        public Pooky(Cookies cookies, PageData pageData, string hostname)
        {
            Cookies = cookies;
            PageData = pageData;
            Hostname = hostname;
        }
    }

    public partial struct Cookies
    {
        [JsonProperty("static")]
        public Static Static { get; internal set; }

        [JsonProperty("atc")]
        public Atc Atc { get; internal set; }

        [JsonProperty("checkout")]
        public Atc Checkout { get; internal set; }

        [JsonConstructor]
        public Cookies(Static @static, Atc atc, Atc checkout)
        {
            Static = @static;
            Atc = atc;
            Checkout = checkout;
        }
    }

    public partial struct Atc
    {
        [JsonProperty("updated_pooky_coherence")]
        public string UpdatedPookyCoherence { get; internal set; }

        [JsonProperty("pooky_mouse")]
        public string PookyMouse { get; internal set; }

        [JsonProperty("pooky_electric")]
        public string PookyElectric { get; internal set; }

        [JsonProperty("pooky_performance")]
        public string PookyPerformance { get; internal set; }

        [JsonProperty("pooky_settings")]
        public string PookySettings { get; internal set; }

        [JsonProperty("pooky_data")]
        public string PookyData { get; internal set; }

        [JsonProperty("pooky_recaptcha_coherence")]
        public string PookyRecaptchaCoherence { get; internal set; }

        [JsonProperty("pooky_recaptcha")]
        public string PookyRecaptcha { get; internal set; }

        [JsonProperty("pooky_telemetry")]
        public string PookyTelemetry { get; internal set; }

        [JsonConstructor]
        public Atc(string updatedPookyCoherence,
                   string pookyMouse,
                   string pookyElectric,
                   string pookyPerformance,
                   string pookySettings,
                   string pookyData,
                   string pookyRecaptchaCoherence,
                   string pookyRecaptcha,
                   string pookyTelemetry)
        {
            UpdatedPookyCoherence = updatedPookyCoherence;
            PookyMouse = pookyMouse;
            PookyElectric = pookyElectric;
            PookyPerformance = pookyPerformance;
            PookySettings = pookySettings;
            PookyData = pookyData;
            PookyRecaptchaCoherence = pookyRecaptchaCoherence;
            PookyRecaptcha = pookyRecaptcha;
            PookyTelemetry = pookyTelemetry;
        }
    }

    public partial struct Static
    {
        [JsonProperty("pooky")]
        public Guid Pooky { get; internal set; }

        [JsonProperty("pooky_order_allow")]
        public string PookyOrderAllow { get; internal set; }

        [JsonProperty("pooky_use_cookie")]
        [JsonConverter(typeof(ParseStringConverter))]
        public bool PookyUseCookie { get; internal set; }

        [JsonProperty("_supreme_sess")]
        public string SupremeSess { get; internal set; }

        [JsonConstructor]
        public Static(Guid pooky, string pookyOrderAllow, bool pookyUseCookie, string supremeSess)
        {
            Pooky = pooky;
            PookyOrderAllow = pookyOrderAllow;
            PookyUseCookie = pookyUseCookie;
            SupremeSess = supremeSess;
        }
    }

    public partial struct PageData
    {
        [JsonProperty("mappings")]
        public Mapping[] Mappings { get; internal set; }

        [JsonProperty("siteKey")]
        public string SiteKey { get; internal set; }

        [JsonProperty("region")]
        public string Region { get; internal set; }

        [JsonProperty("cart")]
        public Cart Cart { get; internal set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; internal set; }

        [JsonConstructor]
        public PageData(Mapping[] mappings, string siteKey, string region, Cart cart, DateTimeOffset date)
        {
            Mappings = mappings;
            SiteKey = siteKey;
            Region = region;
            Cart = cart;
            Date = date;
        }
    }

    public partial struct Cart
    {
        [JsonProperty("url")]
        public string Url { get; internal set; }

        [JsonProperty("properties")]
        public Property[] Properties { get; internal set; }

        [JsonConstructor]
        public Cart(string url, Property[] properties)
        {
            Url = url;
            Properties = properties;
        }
    }

    public partial struct Property {
        [JsonProperty("key")]
        public string Key { get; internal set; }

        [JsonProperty("value")]
        public string Value { get; internal set; }

        [JsonProperty("literal")]
        public bool Literal { get; internal set; }

        [JsonConstructor]
        public Property(string key, string value, bool literal)
        {
            Key = key;
            Value = value;
            Literal = literal;
        }
    }

    public partial struct Mapping
    {
        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("mapping")]
        public string MappingMapping { get; internal set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string? Value { get; internal set; }

        [JsonConstructor]
        public Mapping(string name, string mappingMapping, string? value)
        {
            Name = name;
            MappingMapping = mappingMapping;
            Value = value;
        }
    }

    public partial struct Pooky
    {
        public static Pooky FromJson(string json) => JsonConvert.DeserializeObject<Pooky>(json, Converter.Settings);
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

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object? ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            bool b;
            if (Boolean.TryParse(value, out b))
            {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}