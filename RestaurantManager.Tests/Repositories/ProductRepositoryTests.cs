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

        private readonly Mock<ICsvFileManager<Product>> _mockCsvFileManager;
        private readonly Mock<IFileWrapper> _mockFileWrapper;

        public ProductRepositoryTests()
        {
            this._mockCsvFileManager = new Mock<ICsvFileManager<Product>>();
            this._mockFileWrapper = new Mock<IFileWrapper>();
        }

        private ProductRepository CreateProductRepository()
        {
            return new ProductRepository(
                this._mockCsvFileManager.Object,
                this._mockFileWrapper.Object);
        }

        [Fact]
        public void Add_FileExists_AppendsProductToCsvFile()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            Product product = new Product { Name = "test", PortionCount = 5, Unit = "kg", PortionSize = 0.5 };
            _mockCsvFileManager.Setup(f => f.ReadFromFile(It.IsAny<string>())).Returns(new List<Product> { product });
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            // Act
            productRepository.Add(product);
            // Assert
            _mockCsvFileManager.Verify(manager => manager.AppendToFile(It.IsAny<string>(), It.IsAny<Product>()), Times.Once);
        }
        [Fact]
        public void Add_FileDoesNotExist_WritesProductToCsvFile()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            Product product = new Product { Name = "test", PortionCount = 5, Unit = "kg", PortionSize = 0.5 };
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);
            // Act
            productRepository.Add(product);
            // Assert
            _mockCsvFileManager.Verify(manager => manager.WriteToFile(It.IsAny<string>(), It.IsAny<List<Product>>()), Times.Once);
        }

        [Fact]
        public void GetAll_FileExists_ReturnProductList()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            var expected = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Name = "test1",
                    PortionCount = 1,
                    PortionSize = 1.1,
                    Unit = "kg",
                },
                new Product
                {
                    Id = 2,
                    Name = "test2",
                    PortionCount = 2,
                    PortionSize = 2.1,
                    Unit = "kg",
                }
            };
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);
            // Act
            var result = productRepository.GetAll();

            // Assert
            Assert.True(result.Count == 2);
        }
        [Fact]
        public void GetAll_FileDoesNotExist_ReturnEmptyList()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(new List<Product>());

            // Act
            var result = productRepository.GetAll();

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void Remove_IfProductExist_RemovesProductAndWritesToFile()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            int id = 1;
            var productToRemove = new Product
            {
                Id = 1,
                Name = "test1",
                PortionCount = 1,
                PortionSize = 1.1,
                Unit = "kg",
            };
            var expectedList = new List<Product>() { productToRemove };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expectedList);

            // Act
            productRepository.Remove(id);

            // Assert
            Assert.True(expectedList.Count == 0);
            _mockCsvFileManager.Verify(c => c.WriteToFile(It.IsAny<string>(), expectedList), Times.Once);
        }

        [Fact]
        public void Update_IfProductExist_UpdatesProductAndWritesToFile()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            int id = 1;
            var newProduct = new Product
            {
                Id = 1,
                Name = "Updated",
                PortionCount = 1,
                PortionSize = 1.1,
                Unit = "kg",
            };
            var expectedList = new List<Product>() { new Product
            {
                Id = 1,
                Name = "test1",
                PortionCount = 1,
                PortionSize = 1.1,
                Unit = "kg",
            } };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expectedList);

            // Act
            productRepository.Update(id, newProduct);

            // Assert
            Assert.True(expectedList[0].Name == newProduct.Name);
            _mockCsvFileManager.Verify(c => c.WriteToFile(It.IsAny<string>(), expectedList), Times.Once);

        }

        [Fact]
        public void ProductsExist_OneOfTheProductIdsDoesNotExist_ReturnsIdListOfProductsThatDoesNotExist()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            var expected = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Name = "test1",
                    PortionCount = 1,
                    PortionSize = 1.1,
                    Unit = "kg",
                },
                new Product
                {
                    Id = 2,
                    Name = "test2",
                    PortionCount = 2,
                    PortionSize = 2.1,
                    Unit = "kg",
                }
            };
            List<int> productIdList = new List<int>() { 1, 3 };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);

            // Act
            var result = productRepository.ProductsExist(productIdList);

            // Assert
            Assert.True(result[0] == 3);
        }

        [Fact]
        public void DeductProducts_IfEmptyProductsExist_ReturnEmptyProductIdList()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            var expected = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Name = "test1",
                    PortionCount = 1,
                    PortionSize = 1.1,
                    Unit = "kg",
                },
                new Product
                {
                    Id = 2,
                    Name = "test2",
                    PortionCount = 0,
                    PortionSize = 2.1,
                    Unit = "kg",
                }
            };
            List<int> productIdList = new List<int>() { 1, 2 };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);


            // Act
            var result = productRepository.DeductProducts(productIdList);

            // Assert
            Assert.True(result[0] == 2);
        }
        [Fact]
        public void DeductProducts_IfEmptyProductsDoesNotExist_DeductPortionCountAndWriteToFile()
        {
            // Arrange
            var productRepository = this.CreateProductRepository();
            var expected = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Name = "test1",
                    PortionCount = 10,
                    PortionSize = 1.1,
                    Unit = "kg",
                },
                new Product
                {
                    Id = 2,
                    Name = "test2",
                    PortionCount = 10,
                    PortionSize = 2.1,
                    Unit = "kg",
                }
            };
            List<int> productIdList = new List<int>() { 1, 2 };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);


            // Act
            var result = productRepository.DeductProducts(productIdList);

            // Assert
            Assert.True(expected[0].PortionCount == 9);
            _mockCsvFileManager.Verify(c => c.WriteToFile(It.IsAny<string>(), expected), Times.Once);
        }
    }
}
