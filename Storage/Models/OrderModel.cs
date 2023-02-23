using System.ComponentModel.DataAnnotations;

namespace Storage.Models {
    public class OrderModel {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        public bool RemoveProduct { get; set; } = false;
    }
}
