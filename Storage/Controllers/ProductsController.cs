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
        public IEnumerable<string> Get() {
            return new string[] { JsonSerializer.Serialize(products) };
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id) {
            return JsonSerializer.Serialize(products[id]);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value) {
            var result = JsonSerializer.Deserialize<ProductModel>(value);
            if(result != null) { 
                products.Add(result);
            }
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
            var result = JsonSerializer.Deserialize<ProductModel>(value);
            if (result != null && id > 0 && id <= products.Count) {
                products.Insert(id, result);
            }
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
            if(id > 0 && id < products.Count) 
                products.RemoveAt(id);
        }
    }
}
