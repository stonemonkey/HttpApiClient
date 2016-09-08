// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Text;

namespace HttpApiClient.Configurations
{
    public class UploadConfig : Config
    {
        private readonly string _type;
        private readonly string _data;

        public UploadConfig(string resource, string data)
            : this(resource, IsSecured, data)
        {
        }

        public UploadConfig(string resource, bool isSecured, string data)
            : this(resource, isSecured, data, DefaultContentType)
        {
        }

        public UploadConfig(string resource, bool isSecured, string data, string type)
            : base(resource, isSecured)
        {
            _data = data;
            _type = type;
        }

        public override HttpContent GetContent()
        {
            return new StringContent(_data, Encoding.UTF8, _type);
        }
    }
}