using System;
using System.Collections.Generic;
using GroceryStore.Entity;
using GroceryStore.Utility;

namespace GroceryStore.DataBroker
{
    /// <summary>
    /// this implementation is to honor json files of the type included in the bare solution.
    /// Although, I can't see a real-world application for this scenario, it works fine for this
    /// exercise.
    /// </summary>
    public class JsonDataBroker : IDataBroker
    {
        public const string CustomerSection = "customers";
        public const string ProductSection = "products";
        public const string OrderSection = "orders";

        private EntityJsonFileManager _fileManager;

        public string FilePath { get; private set; }

        public JsonDataBroker(string filePath)
        {
            FilePath = filePath;
            _fileManager = new EntityJsonFileManager(filePath);
        }
        public Customer GetCustomer(int id)
        {
            return _fileManager.Retrieve<Customer>(CustomerSection, o => o.Id == id);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _fileManager.RetrieveMultiple<Customer>(CustomerSection);
        }

        public Order GetOrder(int id)
        {
            return _fileManager.Retrieve<Order>(OrderSection, o => o.Id == id);
        }

        public IEnumerable<Order> GetOrders()
        {
            return _fileManager.RetrieveMultiple<Order>(OrderSection);
        }

        public IEnumerable<Order> GetOrders(DateTime date)
        {
            return _fileManager.RetrieveMultiple<Order>(OrderSection, o => o.OrderDate == date);
        }

        public IEnumerable<Order> GetOrders(int customerId)
        {
            return _fileManager.RetrieveMultiple<Order>(OrderSection, o => o.CustomerId == customerId);
        }

        public Product GetProduct(int id)
        {
            return _fileManager.Retrieve<Product>(ProductSection, o => o.Id == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _fileManager.RetrieveMultiple<Product>(ProductSection);
        }

        public void SaveCustomer(Customer customer)
        {  
            if (customer.Id == null)
            {
                customer.Id = _fileManager.GetMaxAttrValue(CustomerSection, "id") + 1;
            }            
            _fileManager.Insert(CustomerSection, customer, new KeyValuePair<string, string>("id", customer.Id.ToString()));
        }


        public void SaveOrder(Order order)
        {
            if (order.Id == null)
            {
                order.Id = _fileManager.GetMaxAttrValue(OrderSection, "id") + 1;
            }
            _fileManager.Insert(OrderSection, order, new KeyValuePair<string, string>("id", order.Id.ToString()));
        }

        public void SaveProduct(Product product)
        {
            if (product.Id == null)
            {
                product.Id = _fileManager.GetMaxAttrValue(ProductSection, "id") + 1;
            }
            _fileManager.Insert(ProductSection, product, new KeyValuePair<string, string>("id", product.Id.ToString()));
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer.Id == null)
            {
                throw new InvalidOperationException("cannot update a new customer");

            }
            if (!_fileManager.Update(CustomerSection, customer, new KeyValuePair<string, string>("id", customer.Id.ToString())))
            {
                throw new InvalidOperationException("customer not found");
            }
        }

        public void UpdateOrder(Order order)
        {
            if (order.Id == null)
            {
                throw new InvalidOperationException("cannot update a new order");

            }
            if (!_fileManager.Update(OrderSection, order, new KeyValuePair<string, string>("id", order.Id.ToString())))
            {
                throw new InvalidOperationException("order not found");
            }
        }

        public void UpdateProduct(Product product)
        {
            if (product.Id == null)
            {
                throw new InvalidOperationException("cannot update a new product");

            }
            if (!_fileManager.Update(ProductSection, product, new KeyValuePair<string, string>("id", product.Id.ToString())))
            {
                throw new InvalidOperationException("product not found");
            }
        }
    }
}
