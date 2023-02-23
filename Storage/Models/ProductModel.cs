using System.ComponentModel.DataAnnotations;

namespace Storage.Models {
    public class ProductModel {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
    }
}
