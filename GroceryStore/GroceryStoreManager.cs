using System;
using System.Collections.Generic;
using GroceryStore.DataBroker;
using GroceryStore.Entity;
using GroceryStore.Utility;

namespace GroceryStore
{
    public class GroceryStoreManager
    {
        private IDataBroker _dataBroker;

        public GroceryStoreManager(IDataBroker dataBroker)
        {
            _dataBroker = dataBroker;
        }

        public IEnumerable<Customer> GetCustomers()
        {
           return _dataBroker.GetCustomers();

        }

        public Order CreateOrder()
        {
            var order = Factory.CreateOrder();
            order.CreateCheckPoint();
            return order;
        }

        public Product CreateProduct()
        { 
            var product = Factory.CreateProduct();
            product.CreateCheckPoint();
            return product;
        }

        public Customer CreateCustomer()
        {
            var customer = Factory.CreateCustomer();
            customer.CreateCheckPoint();
            return customer;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _dataBroker.GetOrders();
        }

        public IEnumerable<Order> GetOrders(int customerId)
        {
            return _dataBroker.GetOrders(customerId);
        }

        public IEnumerable<Order> GetOrders(DateTime orderDate)
        {
            return _dataBroker.GetOrders(orderDate);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _dataBroker.GetProducts();
        }

        public Customer GetCustomer(int customerId)
        {
            var customer = _dataBroker.GetCustomer(customerId);
            if (customer != null)
            customer?.CreateCheckPoint();
            return customer;
        }

        public Order GetOrder(int orderId)
        {
            var order = _dataBroker.GetOrder(orderId);
            order?.CreateCheckPoint();
            return order;
        }

        public Product GetProduct(int productId)
        {
            var product = _dataBroker.GetProduct(productId);
            product?.CreateCheckPoint();
            return product;
        }

        public void Save(EntityBase objectToSave)
        {
            if (objectToSave.IsDirty()) {
                objectToSave.Save(_dataBroker);
                objectToSave.CreateCheckPoint();

            }           
        }
    }
}
