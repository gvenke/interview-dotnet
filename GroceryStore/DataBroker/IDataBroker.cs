using System;
using System.Collections.Generic;
using System.Text;
using GroceryStore.Entity;

namespace GroceryStore.DataBroker
{
    public interface IDataBroker
    {
        IEnumerable<Customer> GetCustomers();

        Customer GetCustomer(int id);

        IEnumerable<Order> GetOrders();

        IEnumerable<Order> GetOrders(DateTime date);

        IEnumerable<Order> GetOrders(int customerId);

        Order GetOrder(int id);

        IEnumerable<Product> GetProducts();

        Product GetProduct(int id);

        void SaveCustomer(Customer customer);

        void UpdateCustomer(Customer customer);

        void SaveProduct(Product product);

        void UpdateProduct(Product product);

        void SaveOrder(Order order);

        void UpdateOrder(Order order);
    }
}
