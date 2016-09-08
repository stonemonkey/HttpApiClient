// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HttpApiClient.Configurations
{
    public abstract class ConfigBase
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        public IDictionary<string, string> Headers()
        {
            return _headers;
        }

        public void AddHeader(string name, string value)
        {
            _headers.Remove(name);
            _headers.Add(name, value);
        }

        private readonly Dictionary<string, string> _params = new Dictionary<string, string>();

        public void AddParam(string name, string value)
        {
            _params.Remove(name);

            if (!string.IsNullOrWhiteSpace(value))
            {
                _params.Add(name, value);
            }
        }

        protected bool HasParams()
        {
            return _params.Any();
        }

        protected string Params()
        {
            return string.Join("&", _params
                .Select(arg => arg.Key + '=' + Uri.EscapeDataString(arg.Value)));
        }

        public abstract string Url();

        public abstract HttpContent GetContent();
    }
}