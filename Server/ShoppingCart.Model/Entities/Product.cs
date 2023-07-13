using ShoppingCart.Interfacec;

namespace ShoppingCart.Model.Entities
{
    [Serializable]
    public class Product : IProduct
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Order { get; set; }
        public int Amount { get; set; }
    }
}
