// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace HttpApiClient.Configurations
{
    public class Config : ConfigBase
    {
        protected static string DefaultContentType = "application/json";

        protected const bool IsSecured = true;

        private readonly string _urn;
        private readonly bool _isSecured;

        private const string Http = "http";
        private const string SecuredHttp = "https";
        private const string ProtocolSeparator = "://";

        /// <summary>
        /// Initializes instance with an Unified Resource Locator (URL)
        /// </summary>
        /// <param name="url">The URL e.g. 'http://ste.org/img.png'</param>
        public Config(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentOutOfRangeException(url, "Null, empty or whitespace URL!");
            }
            var parts = url.Split(new [] { ProtocolSeparator }, StringSplitOptions.None);
            if (parts.Count() != 2)
            {
                throw new ArgumentOutOfRangeException(url , "Invalid URL!");
            }
            var protocol = parts[0];
            if (string.Equals(SecuredHttp, protocol, StringComparison.OrdinalIgnoreCase))
            {
                _isSecured = true;
            }
            else if (string.Equals(Http, protocol, StringComparison.OrdinalIgnoreCase))
            {
                _isSecured = false;
            }
            else
            {
                throw new ArgumentOutOfRangeException(url , "Invalid protocol!");
            }
            _urn = parts[1];
        }

        /// <summary>
        /// Initializes instance with an Unified Resource Name (URN) 
        /// </summary>
        /// <param name="urn">The URN e.g. 'ste.org/img.png'</param>
        /// <param name="isSecured">True for HTTPS, false for HTTP</param>
        public Config(string urn, bool isSecured)
        {
            if (string.IsNullOrWhiteSpace(urn))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(urn), "Null, empty or white space URN!");
            }
            if (urn.StartsWith($"{Http}{ProtocolSeparator}", StringComparison.OrdinalIgnoreCase) ||
                urn.StartsWith($"{SecuredHttp}{ProtocolSeparator}", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentOutOfRangeException(
                    urn, "Invalid URN! Try using constructor with URL instead.");
            }

            _urn = urn;
            _isSecured = isSecured;
        }
        
        public override string BuildUrl()
        {
            string url = GetProtocol() + ProtocolSeparator + _urn;
            if (HasParams())
            {
                url += "?" + GetQueryStringParams();
            }
            return url;
        }

        protected string GetProtocol()
        {
            return (_isSecured ? SecuredHttp : Http);
        }

        public override HttpContent GetContent()
        {
            return new StringContent(string.Empty, Encoding.UTF8, DefaultContentType);
        }
    }
}