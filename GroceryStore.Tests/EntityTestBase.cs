using System;
using GroceryStore.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GroceryStore.Tests
{
    /// <summary>
    /// base class for all entity subclass UTs. When you create a new entity sub class
    /// you should also create a UT test that derives from this.
    /// </summary>
    [TestClass]
    public abstract class EntityTestBase<T> where T : EntityBase, new()
    {
        protected T _entity;
        protected UTDataBroker _dataBroker;

        [TestMethod]
        public void CheckPointSingle()
        {
            _entity = new T();
            _entity.CreateCheckPoint();

            Assert.IsNotNull(_entity.CurrentCheckPoint, "checkpoint should not be null");
            Assert.IsNotNull(_entity.CheckPointHistory, "checkpoint history should not be null");
            Assert.AreEqual(1, _entity.CheckPointHistory.Count);
            Assert.AreNotEqual(_entity.CheckPointHistory.Last(), _entity.CurrentCheckPoint, "current checkpoint should not match the previous one");         
        }

        [TestMethod]
        public void CheckPointMultiple()
        {
            _entity = new T();
            _entity.CreateCheckPoint();
            var history = _entity.CheckPointHistory;
           _entity.CreateCheckPoint();


            Assert.AreEqual(2, history.Count);
            Assert.IsTrue(history.First().Key < history.Last().Key, "checkpoint history logged out of order");

            bool invalid = false;
            EntityBase prevCheckPoint = null;
            foreach(var key in history.Keys)
            {
                var curCheckPoint = history[key];
                invalid = prevCheckPoint != null && curCheckPoint.Equals(prevCheckPoint);
                if (invalid)
                {
                    break;
                }
                prevCheckPoint = curCheckPoint;
            }

            Assert.IsFalse(invalid, "all checkpoints should be separate references");
        }

        [TestMethod]
        public void CheckPointHistoryLimit()
        {  
            _entity = new T();
            for (int i = 1; i <= 8; i++)
            {
                _entity.CreateCheckPoint();
            }

            Assert.AreEqual(EntityBase.MaxCheckPoints, _entity.CheckPointHistory.Count);
        }

        public void ClearCheckPoints()
        {
            _entity = new T();
            _entity.CreateCheckPoint();
            _entity.ClearCheckPoints();

            Assert.IsNull(_entity.CurrentCheckPoint);
            Assert.IsTrue(_entity.CheckPointHistory.Count == 0);
        }

        [TestMethod]
        public void CheckPointDirty()
        {
            _entity = new T();           
            _entity.CreateCheckPoint();

            Assert.IsFalse(_entity.IsDirty());

            Edit();

            Assert.IsTrue(_entity.IsDirty());

            _entity.CreateCheckPoint();

            Assert.IsFalse(_entity.IsDirty());
        }

        [TestMethod]
        public void CheckPointsNotCloned()
        {
            _entity = new T();
            _entity.CreateCheckPoint();
            _entity.CreateCheckPoint();
            _entity.CreateCheckPoint();
            foreach(var checkPoint in _entity.CheckPointHistory.Values)
            {
                Assert.AreEqual(0, checkPoint.CheckPointHistory.Count);
                Assert.IsNull(checkPoint.CurrentCheckPoint);
            }
        }

        [TestMethod]
        public void IsDirtyWithNoCheckPoint()
        {
            _entity = new T();

            Assert.ThrowsException<InvalidOperationException>(() => _entity.IsDirty());
        }

        [TestMethod]
        public void IsNew()
        {
            _entity = CreateNew();

            Assert.IsTrue(_entity.IsNew());
        }

        [TestMethod]
        public void IsNotNew()
        {
            _entity = CreateExisting();

            Assert.IsFalse(_entity.IsNew());
        }

        protected abstract T CreateNew();

        protected abstract T CreateExisting();

        public abstract void Changes();

        public abstract void SaveNew();

        public abstract void SaveExisting();

        protected abstract void Edit();

        protected  void SetupDataBroker()
        {
            _dataBroker = new UTDataBroker();
        }
    }
}
