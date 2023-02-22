using Microsoft.AspNetCore.Mvc;
using Storage.Models;
using System.Text.Json;

namespace Storage.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private static List<ProductModel> products = new List<ProductModel>();

        // GET: api/<ProductsController>
        [HttpGet]
        public IEnumerable<ProductModel> Get() {
            return products;
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public ProductModel Get(int id) {
            return products[id];
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] ProductModel product) {
            if(product != null)
                products.Add(product);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ProductModel product) {
            if (product != null && id > 0 && id <= products.Count) 
                products.Insert(id, product);
            
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
            if(id > 0 && id < products.Count) 
                products.RemoveAt(id);
        }
    }
}
