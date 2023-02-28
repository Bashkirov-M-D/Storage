using Storage.Controllers;
using Storage.Data;
using Storage.Models;

namespace StorageApp.Tests {

    [Collection("Sequential")]
    public class ProductsControllerTests {
        [Fact]
        public void Get_AllActiveProducts_ReturnsActiveProducts() {
            TestManager.SetupDb();
            var controller = new ProductsController();
            var sampleProducts = TestManager.ProductsList();

            var actual = controller.Get().ToArray();
            var expected = new ProductModel[] {
                sampleProducts[0],
                sampleProducts[2]
            };

            Assert.Equivalent(expected, actual);
        }

        [Fact]
        public void Get_Id1_ReturnsProduct0() {
            TestManager.SetupDb();
            var controller = new ProductsController();
            var sampleProducts = TestManager.ProductsList();
            int id = 1;

            var actual = controller.Get(id);
            var expected = sampleProducts[id-1];

            Assert.Equivalent(expected, actual);
        }

        [Fact]
        public void Post_NewProduct_ReturnsGivenProductFromDb() {
            TestManager.SetupDb();
            var controller = new ProductsController();
            var product = new ProductModel { Id = 5, Name = "New", Description = "Tasty" };

            controller.Post(product);
            var actual = controller.Get(5);
            var expected = product;

            Assert.Equivalent(expected, actual);
        }

        [Fact]
        public void Put_IdToUpdateAndUpdetedProduct_ReturnsNewProductFromDb() {
            TestManager.SetupDb();
            var controller = new ProductsController();
            var product = new ProductModel { Id = 2, Name = "New", Description = "Tasty", Active = false };
            int id = 2;

            controller.Put(id, product);
            var actual = controller.Get(id);
            var expected = product;

            Assert.Equivalent(expected, actual);
        }

        [Fact]
        public void Put_WrongId_ReturnsNull() {
            TestManager.SetupDb();
            var controller = new ProductsController();
            var product = new ProductModel();
            ProductModel? expected = null;
            int id = 10;

            controller.Put(id, product);
            var actual = controller.Get(id);

            Assert.Equivalent(expected, actual);
        }

        [Fact]
        public void Delete_IdOfProductToDelete_ReturnsNoProductAndNoOrdersWithGivenProductId() {
            TestManager.SetupDb();
            var productsController = new ProductsController();
            List<OrderModel> orders;
            int id = 2;

            productsController.Delete(id);
            var product = productsController.Get(id);
            using(var db = new ApiDbContext()) {
                orders = db.Orders.Where<OrderModel>(o => o.ProductId == id).ToList();
            }

            Assert.True(product == null && orders.Count == 0);
        }
    }
}