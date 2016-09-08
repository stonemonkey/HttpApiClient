// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpApiClient.Exceptions;
using Newtonsoft.Json.Linq;

namespace HttpApiClient.Parsers
{
    public class JsonParser : StringParser
    {
        protected JToken Data;

        public override async Task ParseAsync(HttpResponseMessage httpResponse)
        {
            await base.ParseAsync(httpResponse);

            try
            {
                Data = string.IsNullOrWhiteSpace(Content) ?
                    new JObject() : JToken.Parse(Content);
            }
            catch (Exception) // TODO: maybe catch only json exception
            {
                var request = httpResponse.RequestMessage;
                var method = request.Method.ToString();
                var uri = request.RequestUri.ToString();

                throw new InvalidContentException($"Invalid json content: {method} {uri} => ({GetStatusCode()}) '{Content}'");
            }
        }

        public string GetValue(string key)
        {
            var data = Data as JObject;
            var value = data?[key];
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        public JToken GetData()
        {
            return Data;
        }
    }
}