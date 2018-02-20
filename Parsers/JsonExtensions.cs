// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json.Linq;

namespace HttpApiClient.Parsers
{
    public static class JsonExtensions
    {
        /// <summary>
        /// Determines if token is null or empty.
        /// </summary>
        /// <param name="token">Json.NET token instance.</param>
        /// <returns>True if token is null or empty or doesn't have any value, otherwise false.</returns>
        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == string.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }
}