// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using HttpApiClient.Parsers;

namespace HttpApiClient.Exceptions
{
    public class ServerException : Exception
    {
        public IResponseParser ResponseParser { get; private set; }

        public string GetStatusCode()
        {
            return ResponseParser?.GetStatusCode();
        }

        public ServerException(IResponseParser responseParser)
        {
            ResponseParser = responseParser;
        }

        public ServerException(Exception innerException, string message = null)
            : base(message, innerException)
        {
        }
    }
}