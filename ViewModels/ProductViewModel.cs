﻿namespace GemStore.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public RatingViewModel Rating { get; set; }

    }
}
