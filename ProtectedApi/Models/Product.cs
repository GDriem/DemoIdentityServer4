using System.ComponentModel.DataAnnotations;

namespace ProtectedApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }

    public class ProductUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }


    public class ServiceResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; } = 200; // Default: OK
        public string Message { get; set; }
    }

}
