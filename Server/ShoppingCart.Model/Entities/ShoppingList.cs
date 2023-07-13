using ShoppingCart.Interfacec;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;

namespace ShoppingCart
{
    public class ShoppingList : IShoppingList
    {
        private const int MAX_NAME_LENGHT = 50;

        private Dictionary<string, IProduct> _products { get; set; }
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public ShoppingList()
        {
            _products = new Dictionary<string, IProduct>();
        }

        public Task AddProductAsync(IProduct item)
        {
            return Task.Run(() => AddProduct(item));
        }
        private void AddProduct(IProduct product)
        {
            if (!ValidateProducte(product))
            {
                throw new ValidationException($"Failed to valide product params: name:{product.Name}, price:{product.Price}, amount:{product.Amount}.");
            }

            if (!cacheLock.TryEnterWriteLock(10000))
            {
                throw new Exception($"Failed to acquire lock for product: {product.Name}.");
            }

            try
            {
                if (!_products.TryAdd(product.Name, product))
                {
                    throw new DuplicateNameException("Item already exist in the list"); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add item {product.Name}. Error {ex.Message}");
                throw;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }

}

        public Task<List<IProduct>> GetProductsAsync()
        {
            return Task.Run(() =>
                {
                    if (!cacheLock.TryEnterReadLock(10000))
                    {
                        Console.WriteLine($"Failed to acquire lock for get the products.");
                        return new List<IProduct>();
                    }
                    try
                    {
                        return _products.Values.ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load items. Error {ex.Message}");
                        return new List<IProduct>();
                    }
                    finally
                    {
                        cacheLock.ExitReadLock();
                    }
                }
            ); 
        }

        private bool ValidateProducte(IProduct item)
        {
            if(string.IsNullOrWhiteSpace(item?.Name))
            {
                Console.WriteLine("Item can't be null");
                return false; 
            }

            if (item.Name.Length > MAX_NAME_LENGHT)
            {
                Console.WriteLine("Product name is too long");
                return false;
            }

            if (item.Price <= 0)
            {
                Console.WriteLine("Product price must be bigger than 0");
                return false;
            }

            if (item.Amount <= 0)
            {
                Console.WriteLine("Product amount must be bigger than 0");
                return false;
            }

            string namePattern = @"^[a-zA-Z0-9\s]+$";

            if (!Regex.IsMatch(item.Name, namePattern))
            {
                Console.WriteLine("Invalid product name format.");
                return false;
            }
            return true;
        }


        public Task DeleteProductAsync(string productName)
        {
            return Task.Run(() => DeleteProduct(productName));
        }

        public void DeleteProduct(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new KeyNotFoundException("Device name is empty.");
            }

            if (!cacheLock.TryEnterWriteLock(10000))
            {
                throw new Exception($"Failed to acquire lock for product: {productName}.");
            }
            try
            {
                if(!_products.Remove(productName, out _))
                {
                    throw new KeyNotFoundException($"Device {productName} doesn't exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remove item {productName}. Error {ex.Message}");
                throw;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
    }
}
