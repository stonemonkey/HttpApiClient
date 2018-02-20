// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace HttpApiClient
{
    public interface IResponseLogger
    {
        Task LogAsync(Response response, TimeSpan duration);
    }
}