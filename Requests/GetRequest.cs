// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpApiClient.Configurations;

namespace HttpApiClient.Requests
{
    public class GetRequest : RequestBase<ConfigBase>
    {
        public GetRequest(ConfigBase config)
            : base(config)
        {
        }
        public GetRequest(ConfigBase config, IResponseLogger logger)
            : base(config, logger)
        {
        }

        protected override async Task<HttpResponseMessage> SendRequestAsync(
            HttpClient client, string url, CancellationToken cancellationToken)
        {           
            return await client.GetAsync(url, cancellationToken);
        }
    }
}