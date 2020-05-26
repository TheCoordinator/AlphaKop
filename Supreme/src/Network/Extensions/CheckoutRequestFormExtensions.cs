using AlphaKop.Core.CreditCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using FormValue = System.Collections.Generic.KeyValuePair<string, string>;

namespace AlphaKop.Supreme.Network.Extensions {
    static class CheckoutRequestFormExtensions {
        public static FormUrlEncodedContent ToFormUrlEncodedContent(this CheckoutRequest request, ICreditCardFormatter creditCardFormatter) {
            var allValues = new IEnumerable<FormValue>[] {
                GetDefaultPageDataValues(request),
                GetCookieSubValues(request),
                GetCreditCardValues(request, creditCardFormatter: creditCardFormatter),
                GetCard3DSecureValues(request),
                GetOrderProfileValues(request),
                GetOrderAddressValues(request),
                GetCaptchaValues(request),
            };

            var values = allValues
                .SelectMany(value => value)
                .OrderBy(value => value.Key);

            return new FormUrlEncodedContent(values);
        }

        public static string GetTotalsMobileJSQueryString(this CheckoutTotalsMobileRequest request) {
            var query = HttpUtility.ParseQueryString(string.Empty);

            query["cookie-sub"] = GetCookieSubJsonString(sizeId: request.SizeId, quantity: request.Quantity);
            query["mobile"] = "true";
            query["order[billing_country]"] = request.Profile.Address.CountryCode;

            return query?.ToString() ?? "";
        }

        private static IEnumerable<FormValue> GetDefaultPageDataValues(CheckoutRequest request) {
            var result = (from mapping in request.Pooky.PageData.Mappings
                          where mapping.Mapping == null
                          select new FormValue(mapping.Name, mapping.Value ?? "")).ToList();

            result.RemoveAll(value => value.Key == "store_address");

            result.RemoveAll(value => value.Key == "order[terms]");
            result.Add(new FormValue("order[terms]", "1"));

            return result;
        }

        private static string GetCookieSubJsonString(string sizeId, int quantity) {
            var jsonContent = $@"""{sizeId}"":{quantity}";

            return "{" + jsonContent + "}";
        }

        private static IEnumerable<FormValue> GetCookieSubValues(CheckoutRequest request) {
            var jsonString = GetCookieSubJsonString(sizeId: request.SizeId, quantity: request.Quantity);

            return new FormValue[] {
                new FormValue("cookie-sub", jsonString)
            };
        }

        private static IEnumerable<FormValue> GetCaptchaValues(CheckoutRequest request) {
            var captcha = request.Captcha;
            return new FormValue[] {
                new FormValue("g-recaptcha-response", captcha.Token)
            };
        }

        private static IEnumerable<FormValue> GetCreditCardValues(CheckoutRequest request, ICreditCardFormatter creditCardFormatter) {
            var cardDetails = request.Profile.CardDetails;

            return new FormValue[] {
                new FormValue("credit_card[cnb]", creditCardFormatter.FormatCardNumber(cardDetails.CardData)),
                new FormValue("credit_card[month]", cardDetails.ExpiryMonth),
                new FormValue("credit_card[ovv]", cardDetails.Verification),
                new FormValue("credit_card[type]", cardDetails.CardData.IssuerName.ToLower()),
                new FormValue("credit_card[year]", cardDetails.ExpiryYear)
            };
        }

        private static IEnumerable<FormValue> GetCard3DSecureValues(CheckoutRequest request) {
            if (request.CardinalId == null) {
                return Array.Empty<FormValue>();
            }

            return new FormValue[] {
                new FormValue("cardinal_id", request.CardinalId)
            };
        }

        private static IEnumerable<FormValue> GetOrderProfileValues(CheckoutRequest request) {
            var profile = request.Profile;

            return new FormValue[] {
                new FormValue("order[email]", profile.Email),
                new FormValue("order[tel]", profile.PhoneNumber)
            };
        }

        private static IEnumerable<FormValue> GetOrderAddressValues(CheckoutRequest request) {
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
