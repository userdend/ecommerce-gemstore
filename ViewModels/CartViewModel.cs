namespace GemStore.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
