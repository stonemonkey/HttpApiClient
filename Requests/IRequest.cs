// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using HttpApiClient.Configurations;
using HttpApiClient.Parsers;

namespace HttpApiClient.Requests
{
    public interface IRequest
    {
        ConfigBase Config { get; }

        Exception Exception { get; }

        bool IsSuccessful();

        Task<Response<TResponseParser>> RunAsync<TResponseParser>()
            where TResponseParser : IResponseParser, new();
    }
}