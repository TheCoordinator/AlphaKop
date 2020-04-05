using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace AlphaKop.Supreme.Network.Converters {
    sealed class CookieJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            var dictionary = serializer.Deserialize<IDictionary<string, string>>(reader);
            if (dictionary == null) {
                return null;
            }

            return dictionary
                .Select(pair => new Cookie(name: pair.Key, value: pair.Value));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}