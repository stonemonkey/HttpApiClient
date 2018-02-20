// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace HttpApiClient.Parsers
{
    public static class JsonParserExtensions
    {
        /// <summary>
        /// Deserializes Problemator response.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="parser">Parser associated with the response.</param>
        /// <returns>An instance or default(T) in case of an empty Json.</returns>
        public static T To<T>(this JsonParser parser)
        {
            var jToken = parser.GetData();
            if (jToken.IsNullOrEmpty())
            {
                return default(T);
            }

            return jToken.ToObject<T>();
        }
    }
}