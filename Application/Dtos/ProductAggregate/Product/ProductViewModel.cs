﻿namespace Application.Dtos.ProductAggregate.Product
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Brand { get; set; }
        public string Category { get; set; }
        public long CategoryId { get; set; }
        public string CreationDate { get; set; }
        public double UnitPrice { get; set; }
        public int Count { get; set; }
        public bool IsInStock { get; set; }
    }
}
