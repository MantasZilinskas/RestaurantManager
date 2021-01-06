using Moq;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace RestaurantManager.Tests.Repositories
{
    public class MenuItemRepositoryTests
    {
        private readonly Mock<ICsvFileManager<MenuItem>> _mockCsvFileManager;
        private readonly Mock<IFileWrapper> _mockFileWrapper;

        public MenuItemRepositoryTests()
        {
            this._mockCsvFileManager = new Mock<ICsvFileManager<MenuItem>>();
            this._mockFileWrapper = new Mock<IFileWrapper>();
        }

        private MenuItemRepository CreateMenuItemRepository()
        {
            return new MenuItemRepository(
                this._mockCsvFileManager.Object,
                this._mockFileWrapper.Object);
        }

        [Fact]
        public void Add_FileExists_AppendsMenuItemToCsvFile()
        {
            // Arrange
            var menuItemRepository = this.CreateMenuItemRepository();
            MenuItem menuItem = new MenuItem { Name = "test", Products = new List<int>() { 1, 2 } };
            _mockCsvFileManager.Setup(f => f.ReadFromFile(It.IsAny<string>())).Returns(new List<MenuItem> { menuItem });
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            // Act
            menuItemRepository.Add(menuItem);
            // Assert
            _mockCsvFileManager.Verify(manager => manager.AppendToFile(It.IsAny<string>(), menuItem), Times.Once);
        }
        [Fact]
        public void Add_FileDoesNotExist_WritesMenuItemToCsvFile()
        {
            // Arrange
            var menuItemRepository = this.CreateMenuItemRepository();
            MenuItem menuItem = new MenuItem { Name = "test", Products = new List<int>() { 1, 2 } };
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);
            // Act
            menuItemRepository.Add(menuItem);
            // Assert
            _mockCsvFileManager.Verify(manager => manager.WriteToFile(It.IsAny<string>(), new List<MenuItem>() { menuItem }), Times.Once);
        }

        [Fact]
        public void GetAll_FileExists_ReturnMenuItemList()
        {
            // Arrange
            var menuItemRepository = this.CreateMenuItemRepository();
            var expected = new List<MenuItem>()
            {
                new MenuItem { Id = 1 ,Name = "test", Products = new List<int>() { 1, 2 } },
                new MenuItem { Id = 2 ,Name = "test", Products = new List<int>() { 1, 2 } },
            };
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);
            // Act
            var result = menuItemRepository.GetAll();

            // Assert
            Assert.True(result.Count == 2);
        }
        [Fact]
        public void GetAll_FileDoesNotExist_ReturnEmptyList()
        {
            // Arrange
            var menuItemRepository = this.CreateMenuItemRepository();
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(new List<MenuItem>());

            // Act
            var result = menuItemRepository.GetAll();

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void Remove_IfProductExist_RemovesProductAndWritesToFile()
        {
            // Arrange
            var menuItemRepository = this.CreateMenuItemRepository();
            int id = 1;
            var productToRemove = new MenuItem { Id = id, Name = "test", Products = new List<int>() { 1, 2 } };
            var expectedList = new List<MenuItem>() { productToRemove };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expectedList);

            // Act
            menuItemRepository.Remove(id);

            // Assert
            Assert.True(expectedList.Count == 0);
            _mockCsvFileManager.Verify(c => c.WriteToFile(It.IsAny<string>(), expectedList), Times.Once);
        }

        [Fact]
        public void Update_IfProductExist_UpdatesProductAndWritesToFile()
        {
            // Arrange
            var menuItemRepository = this.CreateMenuItemRepository();
            int id = 1;
            var newProduct = new MenuItem { Id = 1, Name = "updated", Products = new List<int>() { 1, 2 } };
            var expectedList = new List<MenuItem>() 
            { 
                new MenuItem { Id = 1, Name = "test", Products = new List<int>() { 1, 2 } }
            };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expectedList);

            // Act
            menuItemRepository.Update(id, newProduct);

            // Assert
            Assert.True(expectedList[0].Name == newProduct.Name);
            _mockCsvFileManager.Verify(c => c.WriteToFile(It.IsAny<string>(), expectedList), Times.Once);

        }

        [Fact]
        public void MenuItemsExist_OneOfTheMenuItemIdsDoesNotExist_ReturnsIdListOfMenuItemsThatDoesNotExist()
        {
            // Arrange
            var menuItemRepository = this.CreateMenuItemRepository();
            var expected = new List<MenuItem>()
            {
                new MenuItem { Id = 1 ,Name = "test", Products = new List<int>() { 1, 2 } },
                new MenuItem { Id = 2 ,Name = "test", Products = new List<int>() { 1, 2 } },
            };
            List<int> menuItemIdList = new List<int>() { 1, 3 };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);

            // Act
            var result = menuItemRepository.MenuItemsExist(menuItemIdList);

            // Assert
            Assert.True(result[0] == 3);
        }

        [Fact]
        public void GetMenuItemProducts_ReturnsMenuItemProducts()
        {
            // Arrange
            var menuItemRepository = this.CreateMenuItemRepository();
            int menuItemId = 1;
            var expected = new List<MenuItem>()
            {
                new MenuItem { Id = 1 ,Name = "test", Products = new List<int>() { 1, 2 } },
                new MenuItem { Id = 2 ,Name = "test", Products = new List<int>() { 1, 2 } },
            };
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);

            // Act
            var result = menuItemRepository.GetMenuItemProducts(menuItemId);

            // Assert
            Assert.True(result == expected[0].Products);
        }
    }
}
