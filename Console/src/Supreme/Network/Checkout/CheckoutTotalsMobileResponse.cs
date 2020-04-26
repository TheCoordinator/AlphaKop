using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlphaKop.Supreme.Network {
    public struct CheckoutTotalsMobileResponse {
        [JsonProperty("OrderNumber")]
        public string OrderNumber { get; }

        [JsonConstructor]
        public CheckoutTotalsMobileResponse(string orderNumber) {
            OrderNumber = orderNumber;
        }

        internal static CheckoutTotalsMobileResponse FromHtmlContent(string content) {
            var html = new HtmlDocument();
            html.LoadHtml(content);

            var value = html.GetElementbyId("jwt_cardinal").Attributes["value"]?.Value;

            if (value == null) {
                throw new NullReferenceException("Expected value to exist");
            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.ReadJwtToken(value);

            return FromJwtToken(jwtToken);
        }

        private static CheckoutTotalsMobileResponse FromJwtToken(JwtSecurityToken jwtToken) {
            var claim = jwtToken.Claims.First(claim => claim.Type == "Payload");
            var jsonObject = JObject.Parse(claim.Value)?.SelectToken("OrderDetails");

            if (jsonObject == null) {
                throw new NullReferenceException("Expected Json Object to exist");
            }

            return jsonObject.ToObject<CheckoutTotalsMobileResponse>();
        }
    }
}