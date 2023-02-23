using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Data;
using Storage.Models;
using System.Text.Json;

namespace Storage.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {

        // GET: api/<ProductsController>
        [HttpGet]
        public IEnumerable<ProductModel> Get() {
            List<ProductModel> products;

            using (var db = new ApiDbContext()) {
                products = db.Products.Where(p => p.Active == true).ToList();
            }

            return products;
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public ProductModel? Get(int id) {
            ProductModel? product;

            using (var db = new ApiDbContext()) {
                product = db.Products.Find(id);
            }

            return product;
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] ProductModel product) {
            if (product != null) {
                using (var db = new ApiDbContext()) {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }
        }

        // PUT api/<ProductsController>/5
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

        // DELETE api/<ProductsController>/5
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
