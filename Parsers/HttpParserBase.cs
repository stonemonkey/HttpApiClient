// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpApiClient.Parsers
{
    public abstract class HttpParserBase : IResponseParser
    {
        protected HttpResponseMessage HttpResponse;

        public string GetStatusCode()
        {
            ThrowIfUnparsedContent();

            return HttpResponse.StatusCode.ToString("D");
        }

        public bool IsResponseSuccessfull()
        {
            ThrowIfUnparsedContent();

            return HttpResponse.IsSuccessStatusCode;
        }

        public abstract Task ParseAsync(HttpResponseMessage httpResponse);

        public IEnumerable<string> GetResponseParameterValues(string parmeterKey)
        {
            ThrowIfUnparsedContent();

            IEnumerable<string> values;
            HttpResponse.Headers.TryGetValues(parmeterKey, out values);

            return values ?? Enumerable.Empty<string>();
        }

        protected void ThrowIfUnparsedContent()
        {
            if (HttpResponse == null)
            {
                throw new InvalidOperationException("Unparsed content (parse http response)!");
            }
        }
    }

    public abstract class HttpParserBase<T> : HttpParserBase
    {
        protected T Content;

        public override async Task ParseAsync(HttpResponseMessage httpResponse)
        {
            HttpResponse = httpResponse;
            Content = await Fetch(HttpResponse);
        }

        protected abstract Task<T> Fetch(HttpResponseMessage response);
    }
}