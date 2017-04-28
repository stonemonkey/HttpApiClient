// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using HttpApiClient.Exceptions;
using HttpApiClient.Parsers;
using HttpApiClient.Requests;

namespace HttpApiClient
{
    public static class ResponseExtensions
    {
        #region OnSuccessfull

        public static Response<TResponseParser> OnSuccess<TResponseParser>(
            this Response<TResponseParser> response, Action<TResponseParser> action)
                where TResponseParser : IResponseParser
        {
            if (IsSuccessfull(response))
            {
                action(response.TypedParser);
            }

            return response;
        }

        public static async Task<Response<TResponseParser>> OnSuccessAsync<TResponseParser>(
            this Response<TResponseParser> response, Func<TResponseParser, Task> func)
                where TResponseParser : IResponseParser
        {
            if (IsSuccessfull(response))
            {
                await func(response.TypedParser);
            }

            return response;
        }

        public static bool IsSuccessfull<TResponseParser>(this Response<TResponseParser> response)
            where TResponseParser : IResponseParser
        {
            return (response.Request.IsSuccessful() && response.IsSuccessfull());
        }

        #endregion

        #region OnResponseFailure

        public static Response<TResponseParser> OnResponseFailure<TResponseParser>(
            this Response<TResponseParser> response, Action<TResponseParser> action)
                where TResponseParser : IResponseParser
        {
            if (response.IsResponseFailure())
            {
                action(response.TypedParser);
            }

            return response;
        }

        public static async Task<Response<TResponseParser>> OnResponseFailureAsync<TResponseParser>(
            this Response<TResponseParser> response, Func<TResponseParser, Task> func)
                where TResponseParser : IResponseParser
        {
            if (response.IsResponseFailure())
            {
                await func(response.TypedParser);
            }

            return response;
        }
        
        public static Response<TResponseParser> OnResponseFailureThrow<TResponseParser>(
            this Response<TResponseParser> response, Action<TResponseParser> action = null)
                where TResponseParser : IResponseParser
        {
            if (response.IsResponseFailure())
            {
                action?.Invoke(response.TypedParser);

                throw new ServerException(response.TypedParser);
            }

            return response;
        }

        public static async Task<Response<TResponseParser>> OnResponseFailureThrowAsync<TResponseParser>(
            this Response<TResponseParser> response, Func<TResponseParser, Task> func = null)
                where TResponseParser : IResponseParser
        {
            if (response.IsResponseFailure())
            {
                if (func != null)
                {
                    await func(response.TypedParser);
                }

                throw new ServerException(response.TypedParser);
            }

            return response;
        }

        public static bool IsResponseFailure<TResponseParser>(this Response<TResponseParser> response)
            where TResponseParser : IResponseParser
        {
            return (response.Request.IsSuccessful() && !response.IsSuccessfull());
        }

        #endregion

        #region OnRequestFailure

        public static Response<TResponseParser> OnRequestFailure<TResponseParser>(
            this Response<TResponseParser> response, Action<IRequest> action)
                where TResponseParser : IResponseParser
        {
            if (response.IsRequestFailure())
            {
                action(response.Request);
            }

            return response;
        }

        public static async Task<Response<TResponseParser>> OnRequestFailureAsync<TResponseParser>(
            this Response<TResponseParser> response, Func<IRequest, Task> func)
                where TResponseParser : IResponseParser
        {
            if (response.IsRequestFailure())
            {
                await func(response.Request);
            }

            return response;
        }

        public static Response<TResponseParser> OnRequestFailureThrow<TResponseParser>(
            this Response<TResponseParser> response, Action<TResponseParser> action = null)
                where TResponseParser : IResponseParser
        {
            if (response.IsRequestFailure())
            {
                action?.Invoke(response.TypedParser);

                throw new ServerException(response.Request.Exception);
            }

            return response;
        }

        public static async Task<Response<TResponseParser>> OnRequestFailureThrowAsync<TResponseParser>(
            this Response<TResponseParser> response, Func<TResponseParser, Task> func = null)
                where TResponseParser : IResponseParser
        {
            if (response.IsRequestFailure())
            {
                if (func != null)
                {
                    await func(response.TypedParser);
                }

                throw new ServerException(response.Request.Exception);
            }

            return response;
        }

        public static bool IsRequestFailure(this Response response)
        {
            return !response.Request.IsSuccessful();
        }

        public static bool IsRequestTimeout(this Response response)
        {
            return response.Request.Exception is TaskCanceledException;
        }

        #endregion

        #region OnAnyFailure

        public static Response<TResponseParser> OnAnyFailureThrow<TResponseParser>(
            this Response<TResponseParser> response, Action<TResponseParser> action = null)
            where TResponseParser : IResponseParser
        {
            return response.OnRequestFailureThrow(action)
                .OnResponseFailureThrow(action);
        }

        public static async Task OnAnyFailureThrowAsync<TResponseParser>(
            this Response<TResponseParser> response, Func<TResponseParser, Task> func = null)
                where TResponseParser : IResponseParser
        {
            await response.OnRequestFailureThrowAsync(func);
            await response.OnResponseFailureThrowAsync(func);
        }

        #endregion
    }
}