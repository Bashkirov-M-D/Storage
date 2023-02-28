using Storage.Data;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.Tests {
    internal class TestManager {
        public static void SetupDb() {
            using (var db = new ApiDbContext()) {
                db.Products.RemoveRange(db.Products);
                db.Orders.RemoveRange(db.Orders);

                db.Products.AddRange(ProductsList());
                db.Orders.AddRange(OrdersList());

                db.SaveChanges();
            }
        }

        public static List<ProductModel> ProductsList() {
            return new List<ProductModel> {
                new ProductModel() { Id = 1, Name = "1", Price = 10},
                new ProductModel() { Id = 2, Name = "2", Price = 40, Active = false},
                new ProductModel() { Id = 3, Name = "3", Price = 70, Description = "None"},
                new ProductModel() { Id = 4, Name = "4", Price = 100, Active = false}
            };
        }

        public static List<OrderModel> OrdersList() {
            return new List<OrderModel> {
                new OrderModel() { Id = 1, ProductId = 2},
                new OrderModel() { Id = 2, ProductId = 2},
                new OrderModel() { Id = 3, ProductId = 2, RemoveProduct = true},
                new OrderModel() { Id = 4, ProductId = 3},
                new OrderModel() { Id = 5, ProductId = 4, RemoveProduct = true},
            };
        }
    }
}
