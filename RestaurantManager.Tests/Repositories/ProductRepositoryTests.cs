using Moq;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace RestaurantManager.Tests.Repositories
{
    public class ProductRepositoryTests
    {

        private Mock<ICsvFileManager<Product>> mockCsvFileManager;
        private Mock<IFileWrapper> mockFileWrapper;

        public ProductRepositoryTests()
        {
            this.mockCsvFileManager = new Mock<ICsvFileManager<Product>>();
            this.mockFileWrapper = new Mock<IFileWrapper>();
        }

        private ProductRepository CreateProductRepository()
        {
            return new ProductRepository(
                this.mockCsvFileManager.Object,
                this.mockFileWrapper.Object);
        }

        [Fact]
        public void Add_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            Product product = null;

            // Act
            productRepository.Add(
                product);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();

            // Act
            var result = productRepository.GetAll();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Remove_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            int id = 0;

            // Act
            productRepository.Remove(
                id);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            int id = 0;
            Product product = null;

            // Act
            productRepository.Update(
                id,
                product);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void ProductsExist_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            List<int> productIdList = null;

            // Act
            var result = productRepository.ProductsExist(
                productIdList);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void DeductProducts_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            List<int> productIdList = null;

            // Act
            var result = productRepository.DeductProducts(
                productIdList);

            // Assert
            Assert.True(false);
        }
    }
}
