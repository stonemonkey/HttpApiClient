// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Text;

namespace HttpApiClient.Configurations
{
    public class Config : ConfigBase
    {
        protected const string DefaultContentType = "application/json";

        protected const bool IsSecured = true;

        private readonly string _resource;
        private readonly bool _isSecured;

        public Config(string resource)
            : this (resource, IsSecured)
        {
        }

        public Config(string resource, bool isSecured)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentOutOfRangeException(resource);
            }

            _resource = resource;
            _isSecured = isSecured;
        }
       
        public override string Url()
        {
            string url = Protocol() + "://" + _resource;
            if (HasParams())
            {
                url += "?" + Params();
            }
            return url;
        }

        protected string Protocol()
        {
            return (_isSecured ? "https" : "http");
        }

        public override HttpContent GetContent()
        {
            return new StringContent(string.Empty, Encoding.UTF8, DefaultContentType);
        }
    }
}