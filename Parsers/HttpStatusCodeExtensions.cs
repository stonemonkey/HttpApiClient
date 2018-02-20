// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace HttpApiClient.Parsers
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsBadRequest(this string code)
        {
            return string.Equals(code, "400", StringComparison.Ordinal);
        }

        public static bool IsUnauthorized(this string code)
        {
            return string.Equals(code, "401", StringComparison.Ordinal);
        }

        public static bool IsForbidden(this string code)
        {
            return string.Equals(code, "403", StringComparison.Ordinal);
        }

        public static bool IsNotFound(this string code)
        {
            return string.Equals(code, "404", StringComparison.Ordinal);
        }

        public static bool IsConflict(this string code)
        {
            return string.Equals(code, "409", StringComparison.Ordinal);
        }

        public static bool IsServerError(this string code)
        {
            return (code != null) && code.StartsWith("5", StringComparison.Ordinal);
        }
    }
}