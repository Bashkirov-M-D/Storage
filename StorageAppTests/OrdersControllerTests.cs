using Storage.Controllers;
using Storage.Data;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.Tests {

    [Collection("Sequential")]
    public class OrdersControllerTests {
        [Fact]
        public void Get_AllOrders_ReturnsAllOrdersFromDb() {
            TestManager.SetupDb();
            var controller = new OrdersController();
            var sampleOrders = TestManager.OrdersList();

            var actual = controller.Get().ToArray();
            var expected = sampleOrders.ToArray();

            Assert.Equivalent(expected, actual);
        }

        [Fact]
        public void Get_Id1_ReturnsOrdder0() {
            TestManager.SetupDb();
            var controller = new OrdersController();
            var sampleOrders = TestManager.OrdersList();
            int id = 1;

            var actual = controller.Get(id);
            var expected = sampleOrders[id - 1];

            Assert.Equivalent(expected, actual);
        }

        [Fact]
        public void Post_NewOrder_ReturnsGivenOrderFromDb() {
            TestManager.SetupDb();
            var controller = new OrdersController();
            var order = new OrderModel { Id = 6, ProductId = 3 };

            controller.Post(order);
            var actual = controller.Get(6);
            var expected = order;

            Assert.Equivalent(expected, actual);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(6)]
        public void Post_WrongOrders_ReturnsNullsFromDb(int productId) {
            TestManager.SetupDb();
            var controller = new OrdersController();
            var order = new OrderModel { Id = 6, ProductId = productId };

            controller.Post(order);
            var actual = controller.Get(6);

            Assert.Null(actual);
        }

        [Fact]
        public void Put_IdToUpdateAndUpdatedOrder_ReturnsUpdatedOrderFromDb() {
            TestManager.SetupDb();
            var controller = new OrdersController();
            var order = new OrderModel { Id = 2, ProductId = 3 };
            int id = 2;

            controller.Put(id, order);
            var actual = controller.Get(id);
            var expected = order;

            Assert.Equivalent(expected, actual);
        }

        [Theory]
        [InlineData(2, 3, true, false)]
        [InlineData(3, 2, false, true)]
        [InlineData(3, 3, true, false)]
        public void Put_IdsToUpdateAndUpdatedOrdersWithProductAvailabilityChange_ReturnsProductAvailabilityFromDb
            (int id, int productId, bool removeProduct, bool expected) 
        {
            TestManager.SetupDb();
            var controller = new OrdersController();
            var order = new OrderModel { Id = id, ProductId = productId, RemoveProduct = removeProduct };
            bool actual;

            controller.Put(id, order);
            using (var db = new ApiDbContext()) {
                actual = db.Products.Find(order.ProductId).Active;
            }


            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Put_WrongId_ReturnsNull() {
            TestManager.SetupDb();
            var controller = new OrdersController();
            var order = new OrderModel();
            OrderModel? expected = null;
            int id = 10;

            controller.Put(id, order);
            var actual = controller.Get(id);

            Assert.Equivalent(expected, actual);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(3, true)]
        public void Delete_IdsOfOrdersToDelete_ReturnsNoOrdersAndProductStateChanged(int id, bool expecredAvailability) {
            TestManager.SetupDb();
            var controller = new OrdersController();
            int productId;
            bool productAvailability;

            var order = controller.Get(id);
            productId = order.ProductId;
            controller.Delete(id);
            using (var db = new ApiDbContext()) {
                productAvailability = db.Products.Single<ProductModel>(p => p.Id == productId).Active;
            }
            order = controller.Get(id);

            Assert.True(order == null && productAvailability == expecredAvailability);
        }
    }
}
