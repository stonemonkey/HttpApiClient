// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading.Tasks;

namespace HttpApiClient.Parsers
{
    public class StringParser : HttpParserBase<string>
    {
        public string GetContent()
        {
            return Content;
        }

        protected override async Task<string> Fetch(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
    }
}