using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProtectedApi.Models;
using ProtectedApi.Services;

namespace ProtectedApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protege todos los endpoints del controlador
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <returns>Una lista de productos.</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }

        /// <summary>
        /// Obtiene un producto por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>retorna un objeto product</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound(new { message = "Producto no encontrado" });

            return Ok(product);
        }

        /// <summary>
        /// Agrega un producto
        /// </summary>
        /// <param name="product"></param>
        /// <returns>retorna el producto creado</returns>
        [HttpPost]
        public IActionResult Add([FromBody] Product product)
        {
            _productService.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        /// <param name="id"></param>
        /// <returns>retorna </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_productService.Delete(id))
                return NoContent();

            return NotFound(new { message = "Producto no encontrado" });
        }

        /// <summary>
        /// Obtiene productos con soporte para paginación y búsqueda.
        /// </summary>
        /// <param name="search">Término de búsqueda opcional.</param>
        /// <param name="page">Número de página (1 por defecto).</param>
        /// <param name="pageSize">Tamaño de página (10 por defecto).</param>
        /// <returns>Productos paginados.</returns>
        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = _productService.GetPaged(search, page, pageSize);
            return Ok(products);
        }


        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDto">The product data transfer object containing updated information.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] ProductUpdateDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateProductAsync(id, productDto);

            if (!result.Success)
                return result.StatusCode == 404
                    ? NotFound(result.Message)
                    : BadRequest(result.Message);

            return Ok(new { Message = result.Message });
        }


    }
}
