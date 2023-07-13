using System.Collections.Concurrent;

namespace ShoppingCart.Interfacec
{
    /// <summary>
    /// Shoping list contains all the products in the shopping list and all the action that are related to the shoping list.
    /// </summary>
    public interface IShoppingList
    {
        /// <summary>
        /// Add new product to shoping list.
        /// Throw exeption in the following cases:
        ///     Name already exist.
        ///     Failed to acquire lock.
        ///     Failed to validate product info.
        /// </summary>
        /// <param name="item">Get the new product</param>
        public Task AddProductAsync(IProduct product);

        /// <summary>
        /// Delete product to shoping list.
        /// Throw exeption in the following cases:
        ///     Name is empty.
        ///     Product doesn't exist.
        ///     Failed to acquire lock.
        /// </summary>
        /// <param name="item">product name to delete.</param>
        Task DeleteProductAsync(string productName);

        /// <summary>
        /// Get all products from the shoping list.
        /// </summary>
        /// <returns></returns>
        Task<List<IProduct>> GetProductsAsync();
    }
}