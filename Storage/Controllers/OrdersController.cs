using Microsoft.AspNetCore.Mvc;
using Storage.Data;
using Storage.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Storage.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {
        
        /// <summary>
        /// Pulls all orders from Database
        /// </summary>
        /// <returns>All orders</returns>
        [HttpGet]
        public IEnumerable<OrderModel> Get() {
            List<OrderModel> orders;

            using (var db = new ApiDbContext()) {
                orders = db.Orders.ToList();
            }

            return orders;
        }

        /// <summary>
        /// Pulls order, that has given id from Database
        /// </summary>
        /// <param name="id">Id of an order you need to get</param>
        /// <returns>Order with given id or null if such order does not exist in Database</returns>
        [HttpGet("{id}")]
        public OrderModel? Get(int id) {
            OrderModel? order;

            using (var db = new ApiDbContext()) {
                order = db.Orders.Find(id);
            }

            return order;
        }

        /// <summary>
        /// Adds given order to Database, if it requires an available product 
        /// </summary>
        /// <param name="order">Order, that has to be added to Database</param>
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

        /// <summary>
        /// Updates info about existing order, if new required product is available. 
        /// </summary>
        /// <param name="id">Id of an order to update</param>
        /// <param name="newOrder">New info about the order</param>
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

        /// <summary>
        /// Deletes order with given id, makes required product available again, if needed.
        /// </summary>
        /// <param name="id">Id of an order to delete</param>
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
