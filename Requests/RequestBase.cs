// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpApiClient.Configurations;
using HttpApiClient.Parsers;

namespace HttpApiClient.Requests
{
    public abstract class RequestBase<TConfig> : IRequest
        where TConfig : ConfigBase
    {
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);

        protected TConfig TypedConfig;

        public ConfigBase Config => TypedConfig;

        public Exception Exception { get; private set; }

        public virtual bool IsSuccessful()
        {
            return !_cancelactionTokenSource.IsCancellationRequested &&
                   (Exception == null);
        }

        private readonly IResponseLogger _logger;

        protected RequestBase(TConfig config) : this(config, null)
        {
        }

        protected RequestBase(TConfig config, IResponseLogger logger)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            TypedConfig = config;
            _logger = logger;
        }

        private CancellationTokenSource _cancelactionTokenSource;

        public void Cancel()
        {
            if ((_cancelactionTokenSource != null) && 
                !_cancelactionTokenSource.IsCancellationRequested)
            {
                _cancelactionTokenSource.Cancel();
            }
        }

        public async Task<Response<TResponseParser>> RunAsync<TResponseParser>() 
            where TResponseParser : IResponseParser, new()
        {
            _cancelactionTokenSource = new CancellationTokenSource();
            Exception = null;

            var stopwatch = Stopwatch.StartNew();
            Response<TResponseParser> response = null;
            try
            {
                var httpResponse = await SendRequestAsync(
                    CreateClient(), TypedConfig.BuildUrl(), _cancelactionTokenSource.Token);

                response = await CreateResponse<TResponseParser>(httpResponse);
            }
            catch (TaskCanceledException e)
            {
                response = CreateCancelationResponse<TResponseParser>(e);
            }
            catch (HttpRequestException e)
            {
                response = CreateHttpExceptionResponse<TResponseParser>(e);
            }
            finally
            {
                stopwatch.Stop();
                if ((_logger != null) && (response != null))
                {
                    await _logger.LogAsync(response, stopwatch.Elapsed);
                }
            }

            return response;
        }

        private Response<TResponseParser> CreateHttpExceptionResponse<TResponseParser>(HttpRequestException e)
            where TResponseParser : IResponseParser, new()
        {
            var response = new Response<TResponseParser>(this);
            Exception = e;

            return response;
        }

        private Response<TResponseParser> CreateCancelationResponse<TResponseParser>(TaskCanceledException e)
            where TResponseParser : IResponseParser, new()
        {
            var response = new Response<TResponseParser>(this);
            if (e.CancellationToken != _cancelactionTokenSource.Token)
            {
                // it seems that on timeout HttpClient throws TaskCanceledException instead of WebException
                //https://social.msdn.microsoft.com/Forums/en-US/d8d87789-0ac9-4294-84a0-91c9fa27e353/bug-in-httpclientgetasync-should-throw-webexception-not-taskcanceledexception?forum=netfxnetcom
                Exception = e;
            }

            return response;
        }

        private async Task<Response<TResponseParser>> CreateResponse<TResponseParser>(
            HttpResponseMessage httpResponse) 
                where TResponseParser : IResponseParser, new()
        {
            var parser = new TResponseParser();
            await parser.ParseAsync(httpResponse);
            return new Response<TResponseParser>(this, parser);
        }

        protected virtual HttpClient CreateClient()
        {
            var handler = new HttpClientHandler();
            TryAddDecompression(handler);

            var client = new HttpClient(handler, true)
            {
                Timeout = Timeout,
            };
            TryAddCustomHeaders(client);
            
            return client;
        }

        private readonly ICollection<string> _excludedHeaders = new Collection<string>
        {
            "Content-Type", 
        };

        private void TryAddCustomHeaders(HttpClient client)
        {
            var headers = TypedConfig.Headers;
            if (headers != null)
            {
                foreach (var header in headers.Where(header => !_excludedHeaders.Contains(header.Key)))
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        private static void TryAddDecompression(HttpClientHandler handler)
        {
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression =
                    (DecompressionMethods.GZip | DecompressionMethods.Deflate);
            }
        }

        protected abstract Task<HttpResponseMessage> SendRequestAsync(
            HttpClient client, string url, CancellationToken cancellationToken);
    }
}