using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlphaKop.Core.Network.Http {
    public class HttpResponseException : Exception {
        public HttpResponseMessage Response { get; }

        public HttpResponseException(string? message, HttpResponseMessage response) : base(message) {
            Response = response;
        }
    }
}