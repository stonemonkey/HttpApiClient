// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using HttpApiClient.Parsers;
using HttpApiClient.Requests;

namespace HttpApiClient
{
    public class Response
    {
        public IRequest Request { get; }
        public IResponseParser Parser { get; }

        public Response(IRequest request, IResponseParser parser)
        {
            Request = request;
            Parser = parser;
        }
    }

    public class Response<TResponseParser> : Response
        where TResponseParser : IResponseParser
    {
        // TODO [cosmo 2016/2/1]: not very nice to have two properties for the same instance
        public TResponseParser TypedParser => (TResponseParser) Parser;

        public bool IsSuccessfull() => 
            ((Parser != null) && Parser.IsResponseSuccessfull());

        public Response(IRequest request)
            : this (request, default(TResponseParser))
        {
        }

        public Response(IRequest request, TResponseParser responseParser)
            : base(request, responseParser)
        {
        }
    }
}