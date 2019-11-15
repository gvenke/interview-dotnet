using System;
using GroceryStore.Entity;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryStore.Tests
{
    [TestClass]
    public class OrderTests : EntityTestBase<Order>
    {
        private static Random _rnd;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _rnd = new Random();
        }

        protected override void Edit()
        {
            _entity.CustomerId = _rnd.Next();
        }

        protected override Order CreateNew()
        {
            return new Order { OrderDate = DateTime.Parse("1/1/2001"), CustomerId = 1};
        }

        protected override Order CreateExisting()
        {
            return new Order { Id = 1, OrderDate = DateTime.Parse("1/1/2001"), CustomerId = 1 };
        }

        [TestMethod]
        public override void Changes()
        {
            _entity = new Order();
            var history = _entity.CheckPointHistory;
            const int initialCustomerId = 1;
            const int changedCustomerId = 2;
            _entity.CustomerId = initialCustomerId;
            _entity.Items = new List<OrderItem> { new OrderItem { ProductId = 1, Quantity = 1 } };            
            _entity.CreateCheckPoint();
            var currentCheckPoint = (Order)_entity.CurrentCheckPoint;
            var items1 = _entity.Items.ToList();

            Assert.IsTrue(_entity.CustomerId == currentCheckPoint.CustomerId);
            CollectionAssert.AreEqual(_entity.Items.ToArray(), currentCheckPoint.Items.ToArray());

            _entity.CustomerId = changedCustomerId;
            _entity.Items.Add(new OrderItem { ProductId = 2, Quantity = 2 });
            var items2 = _entity.Items.ToArray();

            Assert.IsFalse(_entity.CustomerId == currentCheckPoint.CustomerId);
            CollectionAssert.AreNotEqual(_entity.Items.ToArray(), currentCheckPoint.Items.ToArray());

            _entity.CreateCheckPoint();
            currentCheckPoint = (Order)_entity.CurrentCheckPoint;

            Assert.IsTrue(_entity.CustomerId == currentCheckPoint.CustomerId);
            CollectionAssert.AreEqual(_entity.Items.ToArray(), currentCheckPoint.Items.ToArray());

            var checkPoint1 = (Order)history[history.First().Key];
            var checkPoint2 = (Order)history[history.Last().Key];

            Assert.AreEqual(initialCustomerId, checkPoint1.CustomerId);
            Assert.AreEqual(changedCustomerId, checkPoint2.CustomerId);
            CollectionAssert.AreEqual(items1, checkPoint1.Items.ToArray());
            CollectionAssert.AreEqual(items2, checkPoint2.Items.ToArray());
        }

        [TestMethod]
        public override void SaveNew()
        {
            SetupDataBroker();
            _entity = new Order { CustomerId = 1, Items = new List<OrderItem> { new OrderItem { ProductId = 1, Quantity = 1 } } };
            _entity.Save(_dataBroker);

            Assert.IsTrue(_entity.Id != null);
            Assert.IsTrue(_dataBroker.OrderData.Count == 1);
        }

        [TestMethod]
        public override void SaveExisting()
        {
            SetupDataBroker();
            _entity = new Order { Id = 1, CustomerId = 1, Items = new List<OrderItem> { new OrderItem { ProductId = 1, Quantity = 1 } } };
            var orderId = _entity.Id.GetValueOrDefault();
            _entity.CreateCheckPoint();
            var clone = (Order)_entity.CurrentCheckPoint;
            _dataBroker.OrderData.Add(clone.Id.GetValueOrDefault(), clone);
            _entity.Items.Add(new OrderItem { ProductId = 2, Quantity = 2 });
            _entity.Save(_dataBroker);

            Assert.IsTrue(_entity.Id == orderId);
            Assert.IsTrue(_dataBroker.OrderData.Count == 1);
        }
    }
}
