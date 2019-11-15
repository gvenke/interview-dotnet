using System;
using System.Linq;
using GroceryStore.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryStore.Tests
{
    [TestClass]
    public class CustomerTests : EntityTestBase<Customer>
    {
        private static Random _rnd;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _rnd = new Random();
        }

        protected override void Edit()
        {
            _entity.Name = Guid.NewGuid().ToString();
        }

        protected override Customer CreateNew()
        {
            return new Customer { Name = "joe blow" };
        }

        protected override Customer CreateExisting()
        {
            return new Customer { Id = 1, Name = "joe blow" };
        }

        [TestMethod]
        public override void Changes()
        {
            const string initialName = "initial name";
            const string changedName = "changed name";
            _entity = new Customer();
            var history = _entity.CheckPointHistory;           
            _entity.Name = initialName;
            _entity.CreateCheckPoint();

            Assert.IsTrue(_entity.Name == ((Customer)_entity.CurrentCheckPoint).Name);

            _entity.Name = changedName;

            Assert.IsTrue(_entity.Name != ((Customer)_entity.CurrentCheckPoint).Name);

            _entity.CreateCheckPoint();

            Assert.IsTrue(_entity.Name == ((Customer)_entity.CurrentCheckPoint).Name);

            var checkPoint1 = (Customer)history[history.First().Key];
            var checkPoint2 = (Customer)history[history.Last().Key];

            Assert.AreEqual(initialName, checkPoint1.Name);
            Assert.AreEqual(changedName, checkPoint2.Name);
        }

        [TestMethod]
        public override void SaveNew()
        {
            SetupDataBroker();
            _entity = new Customer();
            const string name = "joe blow";
            _entity.Name = name;
            _entity.Save(_dataBroker);

            Assert.IsTrue(_entity.Id != null);
            Assert.IsTrue(_dataBroker.CustomerData.Count == 1);
        }

        [TestMethod]
        public override void SaveExisting()
        {
            SetupDataBroker();
            _entity = new Customer { Id = 1, Name = "joe blow" };
            var custId = _entity.Id.GetValueOrDefault();
            _entity.CreateCheckPoint();
            var clone = (Customer)_entity.CurrentCheckPoint;
            _dataBroker.CustomerData.Add(clone.Id.GetValueOrDefault(), clone);
            _entity.Name = "jack black";
            _entity.Save(_dataBroker);

            Assert.IsTrue(_entity.Id == custId);
            Assert.IsTrue(_dataBroker.CustomerData.Count == 1);
        }
    }
}
