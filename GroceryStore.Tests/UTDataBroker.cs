using System;
using System.Collections.Generic;
using System.Linq;
using GroceryStore.DataBroker;
using GroceryStore.Entity;
using GroceryStore.Utility;

namespace GroceryStore.Tests
{    
    /// <summary>
    /// I'm using this Fake in leouof traditional mocks because of the added benefit of the baked-in collections
    /// </summary>
    public class UTDataBroker : IDataBroker
    {
        public Dictionary<int, Customer> CustomerData { get; internal set; }
        public Dictionary<int, Product> ProductData { get; internal set; }
        public Dictionary<int, Order> OrderData { get; internal set; }

        public UTDataBroker()
        {
            CustomerData = new Dictionary<int, Customer>();
            ProductData = new Dictionary<int, Product>();
            OrderData = new Dictionary<int,Order>();
        }
        public Customer GetCustomer(int id)
        {
            Customer returnVal;
            CustomerData.TryGetValue(id, out returnVal);
            return returnVal;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return CustomerData.Values;
        }

        public Order GetOrder(int id)
        {
            Order returnVal;
            OrderData.TryGetValue(id, out returnVal);
            return returnVal;
        }

        public IEnumerable<Order> GetOrders()
        {
            return OrderData.Values;
        }

        public IEnumerable<Order> GetOrders(DateTime date)
        {
            return OrderData.Values.Where(o => o.OrderDate == date);
        }

        public IEnumerable<Order> GetOrders(int customerId)
        {
            return OrderData.Values.Where(o => o.CustomerId == customerId);
        }

        public Product GetProduct(int id)
        {
            Product returnVal;
            ProductData.TryGetValue(id, out returnVal);
            return returnVal;
        }

        public IEnumerable<Product> GetProducts()
        {
            return ProductData.Values;
        }

        public void SaveCustomer(Customer customer)
        {
            customer.CreateCheckPoint();
            var keyToRemove = customer.CheckPointHistory.Last().Key;

            // cloning the object to separate it from the original object - thereby simulating a real-life data insertion
            var clone = (Customer)customer.CurrentCheckPoint;

            if (customer.Id == null)
            {
                var values = CustomerData.Values;
                clone.Id = values.Count == 0 ? 1 : values.Max(o => o.Id.GetValueOrDefault()) + 1;
            }            
            try
            {
                CustomerData.Add(clone.Id.GetValueOrDefault(), clone);
            } catch(ArgumentException)
            {
                throw new DuplicateEntityKeyException("duplicate key insertion not allowed", clone.Id);
            }            
            customer.Id = clone.Id;

            //get rid of checkpoint
            customer.CheckPointHistory.Remove(keyToRemove);
        }

        public void SaveOrder(Order order)
        {
            order.CreateCheckPoint();

            // cloning the object to separate it from the original object - thereby simulating a real-life data insertion
            var clone = (Order)order.CurrentCheckPoint;
            var keyToRemove = order.CheckPointHistory.Last().Key;

            if (order.Id == null)
            {
                var values = OrderData.Values;
                clone.Id = values.Count == 0 ? 1 : values.Max(o => o.Id.GetValueOrDefault()) + 1;
            }                        
  
            try
            {
                OrderData.Add(clone.Id.GetValueOrDefault(), clone);
            }
            catch (ArgumentException)
            {
                throw new DuplicateEntityKeyException("duplicate key insertion not allowed", clone.Id);
            }            
            order.Id = clone.Id;

            //get rid of checkpoint
            order.CheckPointHistory.Remove(keyToRemove);
        }

        public void SaveProduct(Product product)
        {
            product.CreateCheckPoint();
            var keyToRemove = product.CheckPointHistory.Last().Key;

            // cloning the object to separate it from the original - thereby simulating a real-life data insertion
            var clone = (Product)product.CurrentCheckPoint;
            if (product.Id == null)
            {
                var values = ProductData.Values;
                clone.Id = values.Count == 0 ? 1 : values.Max(o => o.Id.GetValueOrDefault()) + 1;
            }
            try
            {
                ProductData.Add(clone.Id.GetValueOrDefault(), clone);
            }
            catch (ArgumentException)
            {
                throw new DuplicateEntityKeyException("duplicate key insertion not allowed", clone.Id);
            }            
            product.Id = clone.Id;

            //get rid of checkpoint
            product.CheckPointHistory.Remove(keyToRemove);
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer.Id == null)
            {
                throw new InvalidOperationException("cannot update a new customer");
            }
            var customerId = customer.Id.GetValueOrDefault();
            customer.CreateCheckPoint();
            var keyToRemove = customer.CheckPointHistory.Last().Key;

            // cloning the object to separate it from the original - thereby simulating a real-life data insertion
            var clone = (Customer)customer.CurrentCheckPoint;

            if (!CustomerData.ContainsKey(customerId))
            {
                throw new InvalidOperationException("customer not found");
            }
            CustomerData[customerId] = clone;

            //get rid of checkpoint
            customer.CheckPointHistory.Remove(keyToRemove);
        }

        public void UpdateProduct(Product product)
        {
            if (product.Id == null)
            {
                throw new InvalidOperationException("cannot update a new product");
            }
            var productId = product.Id.GetValueOrDefault();
            product.CreateCheckPoint();
            var keyToRemove = product.CheckPointHistory.Last().Key;

            // cloning the object to separate it from the original - thereby simulating a real-life data insertion
            var clone = (Product)product.CurrentCheckPoint;

            if (!ProductData.ContainsKey(productId))
            {
                throw new InvalidOperationException("product not found");
            }
            ProductData[productId] = clone;

            //get rid of checkpoint
            product.CheckPointHistory.Remove(keyToRemove);
        }

        public void UpdateOrder(Order order)
        {
            if (order.Id == null)
            {
                throw new InvalidOperationException("cannot update a new order");
            }
            var orderId = order.Id.GetValueOrDefault();
            order.CreateCheckPoint();
            var keyToRemove = order.CheckPointHistory.Last().Key;

            // cloning the object to separate it from the original - thereby simulating a real-life data insertion
            var clone = (Order)order.CurrentCheckPoint;

            if (!OrderData.ContainsKey(orderId))
            {
                throw new InvalidOperationException("order not found");
            }
            OrderData[orderId] = clone;

            //get rid of checkpoint
            order.CheckPointHistory.Remove(keyToRemove);
        }
    }
}
