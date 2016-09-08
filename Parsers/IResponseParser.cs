// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading.Tasks;

namespace HttpApiClient.Parsers
{
    public interface IResponseParser
    {
        string GetStatusCode();
        bool IsResponseSuccessfull();
        Task ParseAsync(HttpResponseMessage httpResponse);
    }
}