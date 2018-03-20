// Copyright (c) Costin Morariu. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using HttpApiClient.Configurations;
using HttpApiClient.Parsers;
using HttpApiClient.Requests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpApiClient.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        private const string Server = "localhost:64195/api";

        private IResponseLogger _responseLogger = new DebugResponseLogger();

        [Test]
        public async Task Can_create_new_item()
        {
            var newProduct = new Product
            {
                Id = 0,
                Name = "Coca-Cola 2L",
                Price = 3.99M,
                Category = "Drinks"
            };

            var product = await CreateAsync(newProduct);

            Assert.IsNotNull(product);
            Assert.That(product.Id > 0);
        }

        [Test]
        public async Task Can_update_existing_item()
        {
            var products = await ReadAllAsync();
            var existingProduct = products.First();
            existingProduct.Name = Guid.NewGuid().ToString();

            await UpdateAsync(existingProduct);

            var updatedProduct = await ReadAsync(existingProduct.Id);
            Assert.AreEqual(existingProduct.Id, updatedProduct.Id);
            Assert.AreEqual(existingProduct.Name, updatedProduct.Name);
            Assert.AreEqual(existingProduct.Price, updatedProduct.Price);
            Assert.AreEqual(existingProduct.Category, updatedProduct.Category);
        }

        [Test]
        public async Task Can_delete_existing_item()
        {
            var products = await ReadAllAsync();
            var existingProduct = products.Last();

            await DeleteAsync(existingProduct.Id);

            var config = new Config($"{Server}/products/{existingProduct.Id}", false);
            var request = new GetRequest(config, _responseLogger);
            var response = await request.RunAsync<JsonParser>();
            string statusCode = response.Parser.GetStatusCode();
            Assert.AreEqual("404", statusCode);
        }

        private async Task<Product> CreateAsync(Product newProduct)
        {
            var config = new UploadConfig(
                $"{Server}/products",
                false,
                JsonConvert.SerializeObject(newProduct));
            var request = new PostRequest(config, _responseLogger);

            Product product = null;
            var response = await request.RunAsync<JsonParser>();
            if (response.IsSuccessfull())
            {
                product = response.TypedParser.To<Product>();
            }

            return product;
        }

        private async Task<Product> ReadAsync(int productId)
        {
            var config = new Config($"{Server}/products/{productId}", false);
            var request = new GetRequest(config, _responseLogger);
            var response = await request.RunAsync<JsonParser>();

            Product product = null;
            (await request.RunAsync<JsonParser>())
                .OnSuccess(parser => product = parser.To<Product>())
                .OnAnyFailureThrow();

            return product;
        }

        private async Task<IList<Product>> ReadAllAsync()
        {
            var request = new GetRequest(new Config($"{Server}/products", false), _responseLogger);

            IList<Product> products = null;
            (await request.RunAsync<JsonParser>())
                .OnSuccess(parser => products = parser.To<IList<Product>>())
                .OnAnyFailureThrow();

            return products;
        }

        private async Task UpdateAsync(Product product)
        {
            var config = new UploadConfig(
                $"{Server}/products/{product.Id}",
                false,
                JsonConvert.SerializeObject(product));
            var request = new PutRequest(config, _responseLogger);
            var response = await request.RunAsync<JsonParser>();
            response.OnAnyFailureThrow();
        }

        private async Task DeleteAsync(int productId)
        {
            var config = new Config($"{Server}/products/{productId}", false);
            var deleteRequest = new DeleteRequest(config, _responseLogger);
            var response = await deleteRequest.RunAsync<JsonParser>();
            response.OnAnyFailureThrow();
        }
    }
}
