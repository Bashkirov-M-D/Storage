using Microsoft.AspNetCore.Mvc;
using Storage.Data;
using Storage.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Storage.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {
        // GET: api/<OrdersController>
        [HttpGet]
        public IEnumerable<OrderModel> Get() {
            List<OrderModel> orders;

            using (var db = new ApiDbContext()) {
                orders = db.Orders.ToList();
            }

            return orders;
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public OrderModel? Get(int id) {
            OrderModel? order;

            using (var db = new ApiDbContext()) {
                order = db.Orders.Find(id);
            }

            return order;
        }

        // POST api/<OrdersController>
        [HttpPost]
        public void Post([FromBody] OrderModel order) {
            if (order != null) {
                using (var db = new ApiDbContext()) {
                    ProductModel? orderedProduct = db.Products.Find(order.ProductId);
                    if (orderedProduct != null && orderedProduct.Active) {
                        db.Orders.Add(order);

                        if (order.RemoveProduct)
                            orderedProduct.Active = false;

                        db.SaveChanges();
                    }
                }
            }
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] OrderModel newOrder) {
            if (newOrder != null) {
                using (var db = new ApiDbContext()) {
                    OrderModel? order = db.Orders.Find(id);

                    if (order != null) {
                        ProductModel? orderedProduct = db.Products.Find(newOrder.ProductId);
                        if (orderedProduct != null && orderedProduct.Active) {
                            if (order.RemoveProduct && !newOrder.RemoveProduct)
                                orderedProduct.Active = true;

                            order.ProductId = newOrder.ProductId;
                            order.RemoveProduct = newOrder.RemoveProduct;

                            if (order.RemoveProduct)
                                orderedProduct.Active = false;

                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
            using (var db = new ApiDbContext()) {
                OrderModel? order = db.Orders.Find(id);
                if (order != null) {
                    if (order.RemoveProduct) {
                        ProductModel? orderedProduct = db.Products.Find(order.ProductId);
                        if(orderedProduct != null)
                            orderedProduct.Active = true;
                    }

                    db.Orders.Remove(order);
                    db.SaveChanges();
                }
            }
        }
    }
}
