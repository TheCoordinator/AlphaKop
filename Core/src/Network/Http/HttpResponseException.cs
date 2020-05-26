using System;
using System.Net.Http;

namespace AlphaKop.Core.Network.Http {
    public class HttpResponseException : Exception {
        public HttpResponseMessage Response { get; }

        public HttpResponseException(string? message, HttpResponseMessage response) : base(message) {
            Response = response;
        }
    }
}