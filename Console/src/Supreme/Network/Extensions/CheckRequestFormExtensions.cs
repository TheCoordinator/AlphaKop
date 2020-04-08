using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using AlphaKop.Core.CreditCard;

using FormValue = System.Collections.Generic.KeyValuePair<string, string>;

namespace AlphaKop.Supreme.Network.Extensions {
    static class CheckoutRequestFormExtensions {
        public static FormUrlEncodedContent ToFormUrlEncodedContent(this ICheckoutRequest request, ICreditCardFormatter creditCardFormatter) {
            var allValues = new IEnumerable<FormValue>[] {
                GetDefaultPageDataValues(request),
                GetCookieSubValues(request),
                GetCreditCardValues(request, creditCardFormatter: creditCardFormatter),
                GetOrderProfileValues(request),
                GetOrderAddressValues(request),
                GetCaptchaValues(request)
            };

            var values = allValues
                    .SelectMany(value => value)
                    .OrderBy(value => value.Key);

            var stringValue = string.Join('\n', values.Select(v => v.ToString()));

            return new FormUrlEncodedContent(values);
        }

        private static IEnumerable<FormValue> GetDefaultPageDataValues(ICheckoutRequest request) {
            var result = (from mapping in request.Pooky.PageData.Mappings
                          where mapping.Mapping == null && mapping.Value != null
                          select new FormValue(mapping.Name, mapping.Value ?? "")).ToList();

            result.RemoveAll(value => value.Key == "order[terms]");
            result.Add(new FormValue("order[terms]", "1"));

            return result;
        }

        private static IEnumerable<FormValue> GetCookieSubValues(ICheckoutRequest request) {
            var jsonContent = $@"""{request.SizeId}"":{request.Quantity}";

            var json = "{ " + jsonContent + " }";

            return new FormValue[] {
                new FormValue("cookie-sub", json)
            };
        }

        private static IEnumerable<FormValue> GetCaptchaValues(ICheckoutRequest request) {
            var captcha = request.Captcha;
            return new FormValue[] {
                new FormValue("g-recaptcha-response", captcha.Token)
            };
        }

        private static IEnumerable<FormValue> GetCreditCardValues(ICheckoutRequest request, ICreditCardFormatter creditCardFormatter) {
            var cardDetails = request.Profile.CardDetails;

            return new FormValue[] {
                new FormValue("credit_card[cnb]", creditCardFormatter.FormatCardNumber(cardDetails.CardData)),
                new FormValue("credit_card[month]", cardDetails.ExpiryMonth),
                new FormValue("credit_card[ovv]", cardDetails.Verification),
                new FormValue("credit_card[type]", cardDetails.CardData.IssuerName.ToLower()),
                new FormValue("credit_card[year]", cardDetails.ExpiryYear)
            };
        }

        private static IEnumerable<FormValue> GetOrderProfileValues(ICheckoutRequest request) {
            var profile = request.Profile;

            return new FormValue[] {
                new FormValue("order[email]", profile.Email),
                new FormValue("order[tel]", profile.PhoneNumber)
            };
        }

        private static IEnumerable<FormValue> GetOrderAddressValues(ICheckoutRequest request) {
            var address = request.Profile.Address;

            return new FormValue[] {
                new FormValue("order[billing_address]", address.LineOne),
                new FormValue("order[billing_address_2]", address.LineTwo ?? ""),
                new FormValue("order[billing_address_3]", address.LineThree ?? ""),
                new FormValue("order[billing_city]", address.City),
                new FormValue("order[billing_country]", address.CountryCode),
                new FormValue("order[billing_name]", address.FullName),
                new FormValue("order[billing_zip]", address.PostCode)
            };
        }
    }
}
