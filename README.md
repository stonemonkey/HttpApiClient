# HttpApiClient   
    
Simple client library for parsing HTTP API responses.

__In order to use it, add the following nuget packages to your project (order is important):__
1.   Microsoft.Bcl.Build 1.0.21
2.   Microsoft.Net.Http 2.2.29
3.   HttpApiClient 1.0.5
4.   Newtonsoft.Json 11.0.1 (optional, needed if using JsonParser)

## Samples

Check HttpApiClient.Tests project for running code.

### GET requests

```csharp
    private async Task<Product> ReadAsync(int productId)
    {
        var config = new Config($"localhost:64195/api/products/{productId}", false);
        var request = new GetRequest(config, _responseLogger);

        Product product = null;
        (await request.RunAsync<JsonParser>())
            .OnSuccess(parser => product = parser.To<Product>())
            .OnAnyFailureThrow();

        return product;
    }

    private async Task<IList<Product>> ReadAllAsync()
    {
        var request = new GetRequest(new Config($"localhost:64195/api/products", false), _responseLogger);

        IList<Product> products = null;
        (await request.RunAsync<JsonParser>())
            .OnSuccess(parser => products = parser.To<IList<Product>>())
            .OnAnyFailureThrow();

        return products;
    }
```

### POST request

```csharp
    private async Task<Product> CreateAsync(Product newProduct)
    {
        var config = new UploadConfig(
            $"localhost:64195/api/products",
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
```

### PUT request

```csharp
    private async Task UpdateAsync(Product product)
    {
        var config = new UploadConfig(
            $"localhost:64195/api/products/{product.Id}",
            false,
            JsonConvert.SerializeObject(product));
        var request = new PutRequest(config, _responseLogger);
        var response = await request.RunAsync<JsonParser>();
        response.OnAnyFailureThrow();
    }
```

### DELETE request

```csharp
    private async Task DeleteAsync(int productId)
    {
        var config = new Config($"localhost:64195/api/products/{productId}", false);
        var deleteRequest = new DeleteRequest(config, _responseLogger);
        var response = await deleteRequest.RunAsync<JsonParser>();
        response.OnAnyFailureThrow();
    }
```
