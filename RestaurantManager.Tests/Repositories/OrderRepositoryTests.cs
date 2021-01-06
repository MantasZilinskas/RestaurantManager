using Moq;
using RestaurantManager.Interfaces;
using RestaurantManager.Model;
using RestaurantManager.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace RestaurantManager.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly Mock<ICsvFileManager<Order>> _mockCsvFileManager;
        private readonly Mock<IFileWrapper> _mockFileWrapper;

        public OrderRepositoryTests()
        {
            this._mockCsvFileManager = new Mock<ICsvFileManager<Order>>();
            this._mockFileWrapper = new Mock<IFileWrapper>();
        }

        private OrderRepository CreateOrderRepository()
        {
            return new OrderRepository(this._mockCsvFileManager.Object, this._mockFileWrapper.Object);
        }

        [Fact]
        public void Add_IfFileExists_AppendToFile()
        {
            // Arrange
            var orderRepository = this.CreateOrderRepository();
            Order order = new Order
            {
                Id = 1,
                DateTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"),
                MenuItems = new List<int>() { 1, 2 }
            };
            var expected = new List<Order>()
            {
                new Order
                {
                    Id = 5,
                    DateTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"),
                    MenuItems = new List<int>() { 1, 2 }
                },
            };
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);


            // Act
            orderRepository.Add(order);

            // Assert
            _mockCsvFileManager.Verify(c => c.AppendToFile(It.IsAny<string>(), order), Times.Once);
        }
        [Fact]
        public void Add_IfFileDoesNotExist_WriteToFile()
        {
            var orderRepository = this.CreateOrderRepository();
            Order order = new Order
            {
                Id = 1,
                DateTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"),
                MenuItems = new List<int>() { 1, 2 }
            };
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);

            // Act
            orderRepository.Add(order);

            // Assert
            _mockCsvFileManager.Verify(c => c.WriteToFile(It.IsAny<string>(), new List<Order>() { order }), Times.Once);
        }

        [Fact]
        public void GetAll_IfFileExists_ReturnOrderList()
        {
            // Arrange
            var orderRepository = this.CreateOrderRepository();
            var expected = new List<Order>()
            {
                new Order
                {
                    Id = 5,
                    DateTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"),
                    MenuItems = new List<int>() { 1, 2 }
                },
            };
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);

            // Act
            var result = orderRepository.GetAll();

            // Assert
            Assert.True(result.Count == 1);
        }
        [Fact]
        public void GetAll_IfFileDoesNotExist_ReturnEmptyList()
        {
            // Arrange
            var orderRepository = this.CreateOrderRepository();
            var expected = new List<Order>();
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);
            _mockCsvFileManager.Setup(c => c.ReadFromFile(It.IsAny<string>())).Returns(expected);

            // Act
            var result = orderRepository.GetAll();

            // Assert
            Assert.True(result.Count == 0);
        }
    }
}
