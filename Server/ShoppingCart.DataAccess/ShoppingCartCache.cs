using ShoppingCart.Interfacec;

namespace ShoppingCart.DataAccess
{
    /// <summary>
    /// Shoping Cart - cache. For now contain only one shoping cart. Can be replace by a Dictionary if need more than one. 
    /// Suppose to be connected to DB
    /// </summary>
    public class ShoppingCartCache
    {
        private static readonly IShoppingList _shoppingList = new ShoppingList();

        static ShoppingCartCache()
        {
        }

        private ShoppingCartCache()
        {
        }   
        
        public static IShoppingList ShoppingCart
        { 
            get { 
                return _shoppingList;
            }
        }
    }
}