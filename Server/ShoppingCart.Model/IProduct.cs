namespace ShoppingCart.Interfacec
{

    /// <summary>
    /// Product contain all the data that is relevant to the product.
    /// </summary>
    public interface IProduct
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}
