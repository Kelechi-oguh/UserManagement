namespace UserManagement.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Cart
    {
        public int id { get; set; }
        public List<Product> products { get; set; }
        public int total { get; set; }
        public int discountedTotal { get; set; }
        public int userId { get; set; }
        public int totalProducts { get; set; }
        public int totalQuantity { get; set; }
    }

    public class CartProduct
    {
        public int id { get; set; }
        public string title { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int total { get; set; }
        public double discountPercentage { get; set; }
        public int discountedPrice { get; set; }
        public string thumbnail { get; set; }
    }

    public class CartRoot
    {
        public List<Cart> carts { get; set; }
        public int total { get; set; }
        public int skip { get; set; }
        public int limit { get; set; }
    }


}
