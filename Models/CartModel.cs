using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }
        public string ProductImage { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
    }
}
