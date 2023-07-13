using System.ComponentModel.DataAnnotations;
using Moq;
using System.Data;
using ShoppingCart.Interfacec;

namespace ShoppingCart.Tests
{
    public class ShoppingListTests
    {
        private readonly Mock<IProduct> validProductMock;
        private readonly Mock<IProduct> invalidProductMock;

        public ShoppingListTests()
        {
            validProductMock = new Mock<IProduct>();
            validProductMock.SetupGet(p => p.Name).Returns("Apple");
            validProductMock.SetupGet(p => p.Price).Returns(2.99);
            validProductMock.SetupGet(p => p.Amount).Returns(5);

            invalidProductMock = new Mock<IProduct>();
            invalidProductMock.SetupGet(p => p.Name).Returns("InvalidProduct");
            invalidProductMock.SetupGet(p => p.Price).Returns(0);
            invalidProductMock.SetupGet(p => p.Amount).Returns(10);
        }

        [Fact]
        public async Task AddProductAsync_ValidProduct_SuccessfullyAddsProduct()
        {
            // Arrange
            var shoppingList = new ShoppingList();
            var product = validProductMock.Object;

            // Act
            await shoppingList.AddProductAsync(product);
            var products = await shoppingList.GetProductsAsync();

            // Assert
            Assert.Single(products);
            Assert.Equal(product, products[0]);
        }

        [Fact]
        public async Task AddProductAsync_InvalidProductParams_ThrowsValidationException()
        {
            // Arrange
            var shoppingList = new ShoppingList();
            var product = invalidProductMock.Object;

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await shoppingList.AddProductAsync(product);
            });
        }

        [Fact]
        public async Task AddProductAsync_DuplicateProductName_ThrowsDuplicateNameException()
        {
            // Arrange
            var shoppingList = new ShoppingList();
            var product1 = validProductMock.Object;
            var product2 = validProductMock.Object;

            await shoppingList.AddProductAsync(product1);

            // Act & Assert
            await Assert.ThrowsAsync<DuplicateNameException>(async () =>
            {
                await shoppingList.AddProductAsync(product2);
            });
        }

        [Fact]
        public async Task GetProductsAsync_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var shoppingList = new ShoppingList();

            // Act
            var products = await shoppingList.GetProductsAsync();

            // Assert
            Assert.Empty(products);
        }

        [Fact]
        public async Task DeleteProductAsync_ExistingProductName_SuccessfullyDeletesProduct()
        {
            // Arrange
            var shoppingList = new ShoppingList();
            var product = validProductMock.Object;

            await shoppingList.AddProductAsync(product);

            // Act
            await shoppingList.DeleteProductAsync(product.Name);
            var products = await shoppingList.GetProductsAsync();

            // Assert
            Assert.Empty(products);
        }

        [Fact]
        public async Task DeleteProductAsync_EmptyProductName_ThrowsKeyNotFoundException()
        {
            // Arrange
            var shoppingList = new ShoppingList();

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await shoppingList.DeleteProductAsync(string.Empty);
            });
        }

        [Fact]
        public async Task DeleteProductAsync_NonExistingProductName_ThrowsKeyNotFoundException()
        {
            // Arrange
            var shoppingList = new ShoppingList();
            var productName = "NonExistingProduct";

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await shoppingList.DeleteProductAsync(productName);
            });
        }

    }

}
