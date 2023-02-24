using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Data;
using Storage.Models;
using System.Text.Json;

namespace Storage.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {

        /// <summary>
        /// Pulls all available products from Database
        /// </summary>
        /// <returns>All available products</returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get() {
            List<ProductModel> products;

            using (var db = new ApiDbContext()) {
                products = db.Products.Where(p => p.Active == true).ToList();
            }

            return products;
        }

        /// <summary>
        /// Pulls product with given id from Database
        /// </summary>
        /// <param name="id">Id of product you need to get</param>
        /// <returns>Product with given id or null if such product does not exist in Database</returns>
        [HttpGet("{id}")]
        public ProductModel? Get(int id) {
            ProductModel? product;

            using (var db = new ApiDbContext()) {
                product = db.Products.Find(id);
            }

            return product;
        }

        /// <summary>
        /// Adds given product to Database
        /// </summary>
        /// <param name="product">Product, that has to be added to Database</param>
        [HttpPost]
        public void Post([FromBody] ProductModel product) {
            if (product != null) {
                using (var db = new ApiDbContext()) {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Updates info about existing product
        /// </summary>
        /// <param name="id">Id of a product to update</param>
        /// <param name="newProduct">New info about the product</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ProductModel newProduct) {
            using (var db = new ApiDbContext()) {
                if (newProduct != null) {
                    var product = db.Products.Find(id);

                    if (product != null) {

                        product.Name = newProduct.Name;
                        product.Price = newProduct.Price;
                        product.Description = newProduct.Description;   
                        
                        db.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Deletes product with given id, as well as all orders related to it
        /// </summary>
        /// <param name="id">Id of a product to delete</param>
        [HttpDelete("{id}")]
        public void Delete(int id) {
            using (var db = new ApiDbContext()) {
                ProductModel? product = db.Products.Find(id);
                if (product != null) {
                    db.Orders.RemoveRange(db.Orders.Where<OrderModel>(o => o.ProductId == product.Id));
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }
        }
    }
}
