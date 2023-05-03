using Alba;

using ProductsApi.Adapters;
using ProductsApi.IntegrationTests.Products.Fixtures;
using ProductsApi.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ProductsApi.IntegrationTests.Products;

public class DuplicateProductsAreAssignedUniqueSlug : IClassFixture<ProductsDatabaseFixture>
{
    private readonly IAlbaHost _host;
    private readonly WireMockServer _mockServer;

    public DuplicateProductsAreAssignedUniqueSlug(ProductsDatabaseFixture fixture)
    {
        _host = fixture.AlbaHost;
        _mockServer = fixture.MockServer;
    }

    [Fact]
    public async Task AddingMultipleOfTheSame()
    {
        var request = new CreateProductRequest
        {
            Name = "Super Deluxe Dandruff Shampoo",
            Cost = 120.88M,
            Supplier = new SupplierInformation
            {
                Id = "bobs-shop",
                SKU = "19891"
            }
        };
        _mockServer.Given(Request.Create().WithPath($"/suppliers/{request.Supplier.Id}/products/{request.Supplier.SKU}"))
      .RespondWith(Response.Create()
          .WithStatusCode(200)
          .WithBodyAsJson(new SupplierPricingInformationResponse
          {
              AllowWholesale = true,
              RequiredMsrp = 42.23M
          }
      ));

        await _host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/products");
            api.StatusCodeShouldBe(201);
        });

        var secondResponse = await _host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/products");
            api.StatusCodeShouldBe(201);
        });

        var secondResponseContent = secondResponse.ReadAsJson<CreateProductResponse>();
        Assert.NotNull(secondResponseContent);
        Assert.Equal("super-deluxe-dandruff-shampoo-a", secondResponseContent.Slug);

    }
}
