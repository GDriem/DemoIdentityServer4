using ProtectedApi.Models;
using System.Xml.Linq;

namespace ProtectedApi.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Café", Price = 10.99m },
            new Product { Id = 2, Name = "Té", Price = 5.49m }
        };

        public IEnumerable<Product> GetAll() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }

        public bool Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _products.Remove(product);
                return true;
            }
            return false;
        }

        public IEnumerable<Product> GetPaged(string? search, int page, int pageSize)
        {
            var query = _products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

    }
}
