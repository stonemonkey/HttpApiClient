// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading.Tasks;

namespace HttpApiClient.Parsers
{
    public class ByteArrayParser : HttpParserBase<byte[]>
    {
        public byte[] GetContent()
        {
            return Content;
        }

        protected override async Task<byte[]> Fetch(HttpResponseMessage response)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}