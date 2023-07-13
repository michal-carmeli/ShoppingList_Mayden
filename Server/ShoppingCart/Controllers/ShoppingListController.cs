using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess;
using ShoppingCart.Interfacec;
using ShoppingCart.Model.Entities;


namespace ShoppingCart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingListController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<IProduct>> GetShoppingList()
        {
            return await ShoppingCartCache.ShoppingCart.GetProductsAsync();
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IProduct> AddProductToShoppingListAsync([FromBody] Product product)
        {
            await ShoppingCartCache.ShoppingCart.AddProductAsync(product);
            return product;
        }

        [HttpDelete]
        [Route("DeleteProduct/{productName}")]
        public async Task DeleteProductAsync(string productName)
        {
            await ShoppingCartCache.ShoppingCart.DeleteProductAsync(productName);
        }
    }
}