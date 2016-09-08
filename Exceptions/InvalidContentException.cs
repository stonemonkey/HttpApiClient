// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace HttpApiClient.Exceptions
{
    public class InvalidContentException : Exception
    {
        public InvalidContentException(string message)
            : base(message)
        {
        }
    }
}