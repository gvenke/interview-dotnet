using System;
using System.Collections.Generic;
using GroceryStore.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryStore.Utility;

namespace GroceryStore.Tests
{
    /// <summary>
    ///  base class to be used for all IDataBroker implementation UTs. Helps enforce important
    ///  rules apart from those enforced by the interface contracts.
    /// </summary>
    [TestClass]
    public abstract class DataBrokerTestBase : EntityTestDataBase
    {

        [TestMethod]
        public void GetCustomer()
        {
            SetupDataBroker();
            PopulateCustomerData();
            var customer = _dataBroker.GetCustomer(1);

            Assert.IsNotNull(customer);
            Assert.AreEqual(1, customer.Id);
        }

        [TestMethod]
        public void GetCustomerNotFound()
        {
            SetupDataBroker();
            PopulateCustomerData();
            var customer = _dataBroker.GetCustomer(9);

            Assert.IsNull(customer);
        }

        [TestMethod]
        public void GetCustomers()
        {
            SetupDataBroker();
            PopulateCustomerData();
            var customers = _dataBroker.GetCustomers();

            Assert.IsNotNull(customers);
            Assert.AreEqual(_customers.Length, customers.Count());
        }

        [TestMethod]
        public void GetOrder()
        {
            SetupDataBroker();
            PopulateOrderData();
            var order = _dataBroker.GetOrder(1);

            Assert.IsNotNull(order);
            Assert.AreEqual(1, order.Id);
        }

        [TestMethod]
        public void GetOrderNotFound()
        {
            SetupDataBroker();
            PopulateOrderData();
            var order = _dataBroker.GetOrder(9);

            Assert.IsNull(order);

        }

        [TestMethod]
        public void GetOrders()
        {
            SetupDataBroker();
            PopulateOrderData();
            var orders = _dataBroker.GetOrders();

            Assert.IsNotNull(orders);
            Assert.AreEqual(_orders.Length, orders.Count());
        }

        [TestMethod]
        public void GetOrdersByCustomerId()
        {
            SetupDataBroker();
            PopulateOrderData();
            var orders = _dataBroker.GetOrders(1);

            Assert.IsNotNull(orders);
            Assert.AreEqual(1, orders.Count());
            Assert.AreEqual(1, orders.First().Id);
        }

        [TestMethod]
        public void GetOrdersByDate()
        {
            SetupDataBroker();
            PopulateOrderData();
            var orders = _dataBroker.GetOrders(DateTime.Parse("2/1/2019"));

            Assert.IsNotNull(orders);
            Assert.AreEqual(1, orders.Count());
            Assert.AreEqual(2, orders.First().Id);
        }

        [TestMethod]
        public void GetProduct()
        {
            SetupDataBroker();
            PopulateProductData();
            var product = _dataBroker.GetProduct(1);

            Assert.IsNotNull(product);
            Assert.AreEqual(1, product.Id);
        }

        [TestMethod]
        public void GetProductNotFound()
        {
            SetupDataBroker();
            PopulateProductData();
            var product = _dataBroker.GetProduct(9);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void GetProducts()
        {
            SetupDataBroker();
            PopulateProductData();
            var products = _dataBroker.GetProducts();

            Assert.IsNotNull(products);
            Assert.AreEqual(_products.Length, products.Count());
        }


        [TestMethod]
        public void SaveCustomer()
        {
            SetupDataBroker();
            var newCustomer = new Customer { Name = "Candy Barr" };
            _dataBroker.SaveCustomer(newCustomer);
            var newId = newCustomer.Id;

            Assert.IsNotNull(newId);

            var savedCustomer = _dataBroker.GetCustomer(newId.GetValueOrDefault());

            Assert.IsNotNull(savedCustomer);
            Assert.IsTrue(savedCustomer.Name == newCustomer.Name);
        }

        [TestMethod]
        public void SaveNullCustomer()
        {
            SetupDataBroker();

            Assert.ThrowsException<NullReferenceException>(() => _dataBroker.SaveCustomer(null));
        }

        [TestMethod]
        public void SaveDuplicateCustomer()
        {
            SetupDataBroker();
            PopulateCustomerData();
            var newCustomer = new Customer { Id = 1, Name = "Candy Barr" };

            Assert.ThrowsException<DuplicateEntityKeyException>(() => _dataBroker.SaveCustomer(newCustomer));
        }

        [TestMethod]
        public void UpdateCustomer()
        {
            SetupDataBroker();
            PopulateCustomerData();
            var newCustomer = new Customer {Id = 1, Name = "Candy Barr" };
            _dataBroker.UpdateCustomer(newCustomer);
            var updatedCustomer = _dataBroker.GetCustomer(1);

            Assert.IsNotNull(updatedCustomer);
            Assert.IsTrue(updatedCustomer.Name == newCustomer.Name);
        }

        [TestMethod]
        public void UpdateNullCustomer()
        {
            SetupDataBroker();

            Assert.ThrowsException<NullReferenceException>(() => _dataBroker.UpdateCustomer(null));
        }

        [TestMethod]
        public void UpdateCustomerNullId()
        {
            SetupDataBroker();
            var newCustomer= new Customer { Name = "Candy Barr" };

            Assert.ThrowsException<InvalidOperationException>(() => _dataBroker.UpdateCustomer(newCustomer));
        }

        [TestMethod]
        public void UpdateCustomerNotFound()
        {
            SetupDataBroker();
            PopulateCustomerData();
            var newCustomer = new Customer { Id = 9, Name = "Candy Barr" };

            Assert.ThrowsException<InvalidOperationException>(() => _dataBroker.UpdateCustomer(newCustomer));
        }

        [TestMethod]
        public void SaveOrder()
        {
            SetupDataBroker();
            var newOrder = new Order { CustomerId = 1, Items = new List<OrderItem> { new OrderItem { ProductId = 2, Quantity = 1 } } };
            _dataBroker.SaveOrder(newOrder);
            var newId = newOrder.Id;

            Assert.IsNotNull(newId);

            var savedOrder = _dataBroker.GetOrder(newId.GetValueOrDefault());

            Assert.IsNotNull(savedOrder);
            Assert.IsTrue(newOrder.CustomerId == savedOrder.CustomerId && newOrder.OrderDate == savedOrder.OrderDate && newOrder.Items.Count == savedOrder.Items.Count);
        }

        [TestMethod]
        public void SaveNullOrder()
        {
            SetupDataBroker();

            Assert.ThrowsException<NullReferenceException>(() => _dataBroker.SaveOrder(null));
        }

        [TestMethod]
        public void SaveDuplicateOrder()
        {
            SetupDataBroker();
            PopulateOrderData();
            var newOrder = new Order { Id = 1, CustomerId = 1, Items = new List<OrderItem> { new OrderItem { ProductId = 2, Quantity = 1 } } };

            Assert.ThrowsException<DuplicateEntityKeyException>(() => _dataBroker.SaveOrder(newOrder));
        }


        [TestMethod]
        public void UpdateOrder()
        {
            SetupDataBroker();
            PopulateOrderData();
            var newOrder = new Order { Id = 1, CustomerId = 1, Items = new List<OrderItem> { new OrderItem { ProductId = 2, Quantity = 1 } } };
            _dataBroker.UpdateOrder(newOrder);
            var newId = newOrder.Id;

            Assert.IsNotNull(newId);

            var updatedOrder = _dataBroker.GetOrder(newId.GetValueOrDefault());

            Assert.IsNotNull(updatedOrder);
            Assert.IsTrue(newOrder.CustomerId == updatedOrder.CustomerId && newOrder.OrderDate == updatedOrder.OrderDate && newOrder.Items.Count == updatedOrder.Items.Count);
        }

        [TestMethod]
        public void UpdateNullOrder()
        {
            SetupDataBroker();

            Assert.ThrowsException<NullReferenceException>(() => _dataBroker.UpdateOrder(null));
        }


        [TestMethod]
        public void UpdateOrderNullId()
        {
            SetupDataBroker();
            var newOrder = new Order { CustomerId = 1, Items = new List<OrderItem> { new OrderItem { ProductId = 2, Quantity = 1 } } };

            Assert.ThrowsException<InvalidOperationException>(() => _dataBroker.UpdateOrder(newOrder));
        }

        [TestMethod]
        public void UpdateOrderNotFound()
        {
            SetupDataBroker();
            PopulateOrderData();
            var newOrder = new Order { Id = 9, CustomerId = 1, Items = new List<OrderItem> { new OrderItem { ProductId = 2, Quantity = 1 } } };

            Assert.ThrowsException<InvalidOperationException>(() => _dataBroker.UpdateOrder(newOrder));
        }

        [TestMethod]
        public void SaveProduct()
        {
            SetupDataBroker();
            var newProduct = new Product { Price = 2.99, Description = "test desc" };
            _dataBroker.SaveProduct(newProduct);
            var newId = newProduct.Id;

            Assert.IsNotNull(newId);

            var savedProduct = _dataBroker.GetProduct(newId.GetValueOrDefault());

            Assert.IsNotNull(savedProduct);
            Assert.IsTrue(savedProduct.Price == newProduct.Price && savedProduct.Description == newProduct.Description);
        }

        [TestMethod]
        public void SaveNullProduct()
        {
            SetupDataBroker();

            Assert.ThrowsException<NullReferenceException>(() => _dataBroker.SaveProduct(null));
        }

        [TestMethod]
        public void SaveDuplicateProduct()
        {
            SetupDataBroker();
            PopulateProductData();
            var newProduct = new Product { Id = 1, Price = 2.99, Description = "test desc" };

            Assert.ThrowsException<DuplicateEntityKeyException>(() => _dataBroker.SaveProduct(newProduct));
        }

        [TestMethod]
        public void UpdateProduct()
        {
            SetupDataBroker();
            PopulateProductData();
            var newProduct = new Product { Id = 1, Price = 3.99, Description = "updated test desc" };
            _dataBroker.UpdateProduct(newProduct);
            var updatedProduct = _dataBroker.GetProduct(1);

            Assert.IsNotNull(updatedProduct);
            Assert.IsTrue(updatedProduct.Price == newProduct.Price && updatedProduct.Description == newProduct.Description);
        }

        [TestMethod]
        public void UpdateNullProduct()
        {
            SetupDataBroker();

            Assert.ThrowsException<NullReferenceException>(() => _dataBroker.UpdateProduct(null));
        }

        [TestMethod]
        public void UpdateProductNullId()
        {
            SetupDataBroker();
            var newProduct = new Product { Price = 2.99, Description = "test desc" };

            Assert.ThrowsException<InvalidOperationException>(() => _dataBroker.UpdateProduct(newProduct));
        }

        [TestMethod]
        public void UpdateProductNotFound()
        {
            SetupDataBroker();
            PopulateProductData();
            var newProduct = new Product { Id = 9, Price = 2.99, Description = "test desc" };

            Assert.ThrowsException<InvalidOperationException>(() => _dataBroker.UpdateProduct(newProduct));
        }
    }
}
