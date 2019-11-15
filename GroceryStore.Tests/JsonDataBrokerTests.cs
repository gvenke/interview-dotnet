using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryStore.DataBroker;
using System.IO;
using GroceryStore.Utility;

namespace GroceryStore.Tests
{
    /// <summary>
    /// Summary description for JsonDataBrokerTests
    /// </summary>
    [TestClass]
    public class JsonDataBrokerTests : DataBrokerTestBase
    {

        protected override IDataBroker CreateDataBroker()
        {
            var fileName = $"{Directory.GetCurrentDirectory()}\\{Guid.NewGuid().ToString()}.json";
            EntityJsonFileManager.CreateFile(fileName);
            return new JsonDataBroker(fileName);
        }

        protected override void PopulateCustomerData()
        {
            new EntityJsonFileManager(((JsonDataBroker)_dataBroker).FilePath).Insert("customers", _customers);
        }

        protected override void PopulateOrderData()
        {
            new EntityJsonFileManager(((JsonDataBroker)_dataBroker).FilePath).Insert("orders", _orders);
        }

        protected override void PopulateProductData()
        {
            new EntityJsonFileManager(((JsonDataBroker)_dataBroker).FilePath).Insert("products", _products);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            File.Delete(((JsonDataBroker)_dataBroker).FilePath);
        }

    }
}
