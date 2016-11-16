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
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public void AddHeader(string name, string value)
        {
            Headers.Remove(name);

            if (!string.IsNullOrWhiteSpace(value))
            {
                Headers.Add(name, value);
            }
        }

        public Dictionary<string, string> Params { get; } = new Dictionary<string, string>();

        public void AddParam(string name, string value)
        {
            Params.Remove(name);

            if (!string.IsNullOrWhiteSpace(value))
            {
                Params.Add(name, value);
            }
        }

        protected bool HasParams()
        {
            return Params.Any();
        }

        protected string GetQueryStringParams()
        {
            return string.Join("&", Params
                .Select(arg => $"{arg.Key}={Uri.EscapeDataString(arg.Value)}"));
        }

        public abstract string BuildUrl();

        public abstract HttpContent GetContent();
    }
}