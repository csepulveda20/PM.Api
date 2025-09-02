using System;

namespace PM.Domain.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public string AltText { get; set; }
        public int SortOrder { get; set; }
    }
}