using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryStore.Tests
{
    [TestClass]
    public abstract class GroceryStoreManagerTestBase : EntityTestDataBase
    {
        private GroceryStoreManager _groceryStore;

        [TestMethod]
        public void CreateCustomer()
        {
            SetupGroceryStore();
            var customer = _groceryStore.CreateCustomer();

            Assert.AreEqual(1, customer.CheckPointHistory.Count);
        }

        [TestMethod]
        public void CreateProduct()
        {
            SetupGroceryStore();
            var product = _groceryStore.CreateProduct();

            Assert.AreEqual(1, product.CheckPointHistory.Count);
        }

        [TestMethod]
        public void CreateOrder()
        {
            SetupGroceryStore();
            var order= _groceryStore.CreateOrder();

            Assert.AreEqual(1, order.CheckPointHistory.Count);
        }


        [TestMethod]
        public void GetCustomers()
        {
            SetupGroceryStore();
            PopulateCustomerData();
            var customerData = _groceryStore.GetCustomers();

            Assert.AreEqual(_customers.Count(), customerData.Count());
        }

        [TestMethod]
        public void GetCustomer()
        {
            SetupGroceryStore();
            PopulateCustomerData();
            var customer = _groceryStore.GetCustomer(1);

            Assert.IsNotNull(customer, "customer should not be null");
            Assert.AreEqual(1, customer.Id, "wrong customer retrieved");
            Assert.IsNotNull(customer.CurrentCheckPoint, "customer should have a checkpoint");
        }

        [TestMethod]
        public void GetCustomerNotFound()
        {
            SetupGroceryStore();
            PopulateCustomerData();
            var customer = _groceryStore.GetCustomer(9);

            Assert.IsNull(customer);
        }

        [TestMethod]
        public void GetProduct()
        {
            SetupGroceryStore();
            PopulateProductData();
            var product = _groceryStore.GetProduct(1);

            Assert.IsNotNull(product, "product should not be null");
            Assert.AreEqual(1, product.Id, "wrong product retrieved");
            Assert.IsNotNull(product.CurrentCheckPoint, "product should have a checkpoint");
        }

        [TestMethod]
        public void GetProductNotFound()
        {
            SetupGroceryStore();
            PopulateProductData();
            var product = _groceryStore.GetProduct(9);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void GetOrder()
        {
            SetupGroceryStore();
            PopulateOrderData();
            var order = _groceryStore.GetOrder(1);

            Assert.IsNotNull(order, "order should not be null");
            Assert.AreEqual(1, order.Id, "wrong order retrieved");
            Assert.IsNotNull(order.CurrentCheckPoint, "order should have a checkpoint");
        }

        [TestMethod]
        public void GetOrderNotFound()
        {
            SetupGroceryStore();
            PopulateOrderData();
            var order = _groceryStore.GetOrder(9);

            Assert.IsNull(order);
        }

        [TestMethod]
        public void GetCustomersNotFound()
        {
            SetupGroceryStore();
            var customerData = _groceryStore.GetCustomers();

            Assert.AreEqual(0, customerData.Count());
        }

        [TestMethod]
        public void GetProducts()
        {
            SetupGroceryStore();
            PopulateProductData();
            var productData = _groceryStore.GetProducts();

            Assert.AreEqual(_products.Count(), productData.Count());
        }

        [TestMethod]
        public void GetAllOrders()
        {
            SetupGroceryStore();
            PopulateOrderData();
            var orderData = _groceryStore.GetOrders();

            Assert.AreEqual(_orders.Count(), orderData.Count());
        }

        [TestMethod]
        public void GetAllOrdersNotFound()
        {
            SetupGroceryStore();
            var orderData = _groceryStore.GetOrders();

            Assert.AreEqual(0, orderData.Count());
        }

        [TestMethod]
        public void GetOrdersByCustomer()
        {
            SetupGroceryStore();
            PopulateOrderData();
            var orderData = _groceryStore.GetOrders(2);

            Assert.AreEqual(1, orderData.Count(), "wrong number of orders retrieved");
            Assert.AreEqual(2, orderData.First().Id, "wrong order retrieved");
        }

        [TestMethod]
        public void GetOrdersByCustomerNotFound()
        {
            SetupGroceryStore();
            PopulateOrderData();
            var orderData = _groceryStore.GetOrders(5);

            Assert.AreEqual(0, orderData.Count());

        }

        [TestMethod]
        public void GetOrdersByDate()
        {
            SetupGroceryStore();
            PopulateOrderData();
            var orderData = _groceryStore.GetOrders(DateTime.Parse("2/1/2019"));

            Assert.AreEqual(1, orderData.Count(), "wrong number of orders retrieved");
            Assert.AreEqual(2, orderData.First().Id, "wrong order retrieved");
        }

        [TestMethod]
        public void GetOrdersByDateNotFound()
        {
            SetupGroceryStore();
            PopulateOrderData();
            var orderData = _groceryStore.GetOrders(DateTime.Parse("9/1/2019"));

            Assert.AreEqual(0, orderData.Count());
        }

        [TestMethod]
        public void SaveNull()
        {
            SetupGroceryStore();

            Assert.ThrowsException<NullReferenceException>(() => _groceryStore.Save(null));
        }

        [TestMethod]
        public void SaveNew()
        {
            SetupGroceryStore();
            var customer = _groceryStore.CreateCustomer();
            customer.Name = "joe blow";
            _groceryStore.Save(customer);

            Assert.IsNotNull(customer.Id, "customer Id should not be null");
            Assert.AreEqual(2, customer.CheckPointHistory.Count, "entity should have 2 checkpoints");

            customer = null;
            customer = _groceryStore.GetCustomer(1);

            Assert.IsNotNull(customer, "customer was not saved");
        }

        [TestMethod]
        public void SaveExisting()
        {
            SetupGroceryStore();
            PopulateCustomerData();
            const string newName = "candy Barr";
            var customer = _groceryStore.GetCustomer(1);
            customer.Name = newName;
            _groceryStore.Save(customer);

            Assert.AreEqual(2, customer.CheckPointHistory.Count, "entity should have 2 checkpoints");

            var customers = _groceryStore.GetCustomers();

            Assert.AreEqual(_customers.Length, customers.Count(), "entity was saved, not updated");

            var updatedCustomer = customers.First(o => o.Id == customer.Id);

            Assert.AreEqual(newName, updatedCustomer.Name, "entity was not updated");        
        }

        private void SetupGroceryStore()
        {
            SetupDataBroker();
            _groceryStore = new GroceryStoreManager(_dataBroker);
        }
    }
}
