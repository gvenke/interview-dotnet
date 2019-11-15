using System;
using GroceryStore.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryStore.Tests
{
    [TestClass]
    public class ProductTests : EntityTestBase<Product>
    {
        private static Random _rnd;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _rnd = new Random();
        }

        protected override void Edit()
        {
            _entity.Price= _rnd.NextDouble();
        }

        protected override Product CreateNew()
        {
            return new Product { Price = 1.99, Description = "test desc" };
        }

        protected override Product CreateExisting()
        {
            return new Product { Id = 1, Price = 1.99, Description = "test desc" };
        }

        [TestMethod]
        public override void Changes()
        {
            const string initialDesc = "initial desc";
            const string changedDesc = "changed desc";
            const double initialPrice = 10.25;
            const double changedPrice = 11.50;

            _entity = new Product();
            var history = _entity.CheckPointHistory;
            _entity.Description = initialDesc;
            _entity.Price = initialPrice;
            _entity.CreateCheckPoint();

            Assert.IsTrue(_entity.Description == ((Product)_entity.CurrentCheckPoint).Description, "checkpoint description should match that of the parent product");
            Assert.IsTrue(_entity.Price == ((Product)_entity.CurrentCheckPoint).Price, "checkpoint price should match that of the parent product");

            _entity.Description = changedDesc;
            _entity.Price = changedPrice;

            Assert.IsTrue(_entity.Description != ((Product)_entity.CurrentCheckPoint).Description, "checkpoint description should not match that of the parent product after editing");
            Assert.IsTrue(_entity.Price != ((Product)_entity.CurrentCheckPoint).Price, "checkpoint price should not match that of the parent product after editing");

            _entity.CreateCheckPoint();

            Assert.IsTrue(_entity.Description == ((Product)_entity.CurrentCheckPoint).Description, "checkpoint description should match that of the parent product after creating checkpoint");
            Assert.IsTrue(_entity.Price == ((Product)_entity.CurrentCheckPoint).Price, "checkpoint price should match that of the parent product after creating checkpoint");

            var checkPoint1 = (Product)history[history.First().Key];
            var checkPoint2 = (Product)history[history.Last().Key];

            Assert.AreEqual(initialDesc, checkPoint1.Description, "first checkpoint has the wrong description");
            Assert.AreEqual(changedDesc, checkPoint2.Description, "second checkpoint has the wrong description");
            Assert.AreEqual(initialPrice, checkPoint1.Price, "first checkpoint has the wrong price");
            Assert.AreEqual(changedPrice, checkPoint2.Price, "second checkpoint has the wrong price");
        }

        [TestMethod]
        public override void SaveNew()
        {
            SetupDataBroker();
            _entity = new Product(); 
            _entity.Description = "test desc";
            _entity.Price = 12.99;
            _entity.Save(_dataBroker);

            Assert.IsTrue(_entity.Id != null, "Id should not be null after saving");
            Assert.IsTrue(_dataBroker.ProductData.Count == 1, "product was not saved");
        }

        [TestMethod]
        public override void SaveExisting()
        {
            SetupDataBroker();
            _entity = new Product { Id = 1, Description = "test desc" };
            var productId = _entity.Id.GetValueOrDefault();
            _entity.CreateCheckPoint();
            var clone = (Product)_entity.CurrentCheckPoint;
            _dataBroker.ProductData.Add(clone.Id.GetValueOrDefault(), clone);
            _entity.Description = "changed desc";
            _entity.Save(_dataBroker);

            Assert.IsTrue(_entity.Id == productId, "Id was changed");
            Assert.IsTrue(_dataBroker.ProductData.Count == 1, "product was saved, not updated");
        }
    }
}
