using System.Linq;
using GroceryStore.DataBroker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryStore.Tests
{
    [TestClass]
    public class UTDataBrokerTests : DataBrokerTestBase
    {
        protected override IDataBroker CreateDataBroker()
        {
            return new UTDataBroker();
        }

        protected override void PopulateCustomerData()
        {
            ((UTDataBroker)_dataBroker).CustomerData = _customers.ToDictionary(o => o.Id.GetValueOrDefault());
        }

        protected override void PopulateOrderData()
        {
            ((UTDataBroker)_dataBroker).OrderData = _orders.ToDictionary(o => o.Id.GetValueOrDefault());
        }

        protected override void PopulateProductData()
        {
            ((UTDataBroker)_dataBroker).ProductData = _products.ToDictionary(o => o.Id.GetValueOrDefault());
        }
    }
}
