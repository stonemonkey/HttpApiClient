// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpApiClient.Configurations;

namespace HttpApiClient.Requests
{
    public class PostRequest : RequestBase<ConfigBase>
    {
        public PostRequest(ConfigBase config) 
            : base(config)
        {
        }

        public PostRequest(ConfigBase config, IResponseLogger logger) 
            : base(config, logger)
        {
        }

        protected override async Task<HttpResponseMessage> SendRequestAsync(
            HttpClient client, string url, CancellationToken cancellationToken)
        {
            return await client.PostAsync(url, TypedConfig.GetContent(), cancellationToken);
        }
    }
}
