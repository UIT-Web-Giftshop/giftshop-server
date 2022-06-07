using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Domain.Entities;
using Domain.Paging;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories;
using MongoDB.Driver;
using Moq;
using Web.Tests.Resources;
using Xunit;

namespace Web.Tests
{
    public class ProductRepositoryFixture
    {
        public List<Product> SampleData = ProductResource.ListProducts();
    }
    
    [Collection("ProductRepository")]
    public class ProductRepositoryTests : IClassFixture<ProductRepositoryFixture>
    {
        private readonly IMongoCollection<Product> _mockCollection;
        private readonly IMongoContext _mockContext;
        private readonly ICounterRepository _mockCounterRepository;
        private readonly ProductRepositoryFixture _fixture;
        private readonly IEnumerable<Product> _mockEnumerable;
        private readonly IAsyncCursor<Product> _mockAsyncCursor;

        public ProductRepositoryTests(ProductRepositoryFixture fixture)
        {
            _fixture = fixture;
            _mockCollection = Mock.Of<IMongoCollection<Product>>();
            _mockContext = Mock.Of<IMongoContext>();
            Mock.Get(_mockContext)
                .Setup(x => x.GetCollection<Product>())
                .Returns(_mockCollection);
            _mockCounterRepository = Mock.Of<ICounterRepository>();
            Mock.Get(_mockCounterRepository)
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<CounterCollection, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CounterCollection() { CollectionName = "products", CurrentCount = _fixture.SampleData.Count });
            _mockEnumerable = Mock.Of<IEnumerable<Product>>();
            Mock.Get(_mockEnumerable)
                .Setup(x => x.GetEnumerator())
                .Returns(_fixture.SampleData.GetEnumerator());
            _mockAsyncCursor = Mock.Of<IAsyncCursor<Product>>();
            Mock.Get(_mockAsyncCursor).Setup(x => x.Current).Returns(_mockEnumerable);
            Mock.Get(_mockAsyncCursor)
                .SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            Mock.Get(_mockAsyncCursor)
                .SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
        }

        [Fact]
        [Trait("Category", "ProductRepository")]
        public async void ProductRepository_GetOneProductAsync_ReturnProduct()
        {
            // arrange
            var expectedProduct = _fixture.SampleData[0];
            Expression<Func<Product, bool>> expression = x => x.Id == expectedProduct.Id;
            
            // mock collection find
            Mock.Get(_mockCollection)
                .Setup(x => x.FindAsync(
                    It.IsAny<ExpressionFilterDefinition<Product>>(),
                    It.IsAny<FindOptions<Product,Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockAsyncCursor);
            
            // act
            var repository = new ProductRepository(_mockContext, _mockCounterRepository);
            var result = await repository.GetOneAsync(expression);

            // assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.Name, result.Name);
            Mock.Get(_mockCollection).Verify(x => x.FindAsync(
                It.IsAny<ExpressionFilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product,Product>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        [Trait("Category", "ProductRepository")]
        public async void ProductRepository_GetPagingProducts_ReturnPagingModel()
        {
            // arrange
            Mock.Get(_mockCollection)
                .Setup(x => x.FindAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<FindOptions<Product, Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockAsyncCursor);
            Mock.Get(_mockContext)
                .Setup(x => x.GetCollection<Product>())
                .Returns(_mockCollection);

            // act
            var repo = new ProductRepository(_mockContext, _mockCounterRepository);
            var result = await repo.GetPagingAsync(
                new PagingRequest() { PageSize = 20, PageIndex = 1 },
                null,
                q => q.Price,
                default);

            // assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Mock.Get(_mockCollection)
                .Verify(x => x.FindAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<FindOptions<Product, Product>>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}