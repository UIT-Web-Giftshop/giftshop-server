using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Web.Tests
{
    [Collection("ProductRepository")]
    public class ProductRepositoryTests
    {
        private IMongoCollection<Product> _mockCollection;
        private IMongoContext _mockContext;
        private Product _product;
        private List<Product> _list => new();

        public ProductRepositoryTests()
        {
            _product = new Product()
            {
                Id = "6227538b5f2c8331916327cb",
                Sku = "xUnitTest",
                Name = "xUnit",
                Description = "xUnit Test description",
                Stock = 10
            };
            _list.Add(_product);
            _mockCollection = Mock.Of<IMongoCollection<Product>>();
            _mockContext = Mock.Of<IMongoContext>();
        }

        [Fact]
        [Trait("Category", "ProductRepository")]
        public async void ProductRepository_GetManyProductAsync_ShouldSuccess()
        {
            // arrange
            // mock MoveNext
            var cursor = Mock.Of<IAsyncCursor<Product>>();
            Mock.Get(cursor)
                .Setup(x => x.Current)
                .Returns(_list);
            Mock.Get(cursor)
                .SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            // mock FindAsync
            Mock.Get(_mockCollection)
                .Setup(op =>
                    op.FindAsync(It.IsAny<FilterDefinition<Product>>(),
                        It.IsAny<FindOptions<Product>>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursor);
            // mock GetCollection
            Mock.Get(_mockContext)
                .Setup(x => x.GetCollection<Product>())
                .Returns(_mockCollection);
            // mock SaveFlag
            var saveFlag = Mock.Of<ISaveFlagRepository>();
            var repository = new ProductRepository(_mockContext, saveFlag);

            // act
            var result = await repository.GetManyAsync();
            
            // assert
            foreach (var item in result)
            {
                Assert.NotNull(item);
                Assert.Equal(item.Sku, _product.Sku);
                Assert.Equal(item.Id, _product.Id);
            }
            Mock.Get(_mockCollection)
                .Verify(x => x.FindAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<FindOptions<Product>>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}