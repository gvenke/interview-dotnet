using GroceryStore.DataBroker;
using GroceryStore.Entity;
using System;
using System.Collections.Generic;

namespace GroceryStore.Tests
{
    public abstract class EntityTestDataBase
    {
        protected IDataBroker _dataBroker;
        protected Product[] _products;
        protected Order[] _orders;
        protected Customer[] _customers;

        protected abstract IDataBroker CreateDataBroker();

        protected abstract void PopulateProductData();

        protected abstract void PopulateOrderData();

        protected abstract void PopulateCustomerData();

        protected void SetupDataBroker()
        {
            _dataBroker = CreateDataBroker();
            SetupTestData();
        }

        private  void SetupTestData()
        {
            _products = new[]
            {
                new Product { Id = 1, Description = "test desc 1", Price = 1.99 },
                new Product { Id = 2, Description = "test desc 2", Price = 2.99 },
                new Product { Id = 3, Description = "test desc 3", Price = 3.99 }
            };

            _customers = new[]
            {
                new Customer { Id = 1, Name = "joe blow" },
                new Customer { Id = 2, Name = "jane doe" },
                new Customer { Id = 3, Name = "jack black" }
            };

            _orders = new[]
            {
                new Order { Id = 1, OrderDate = DateTime.Parse("1/1/2019"), CustomerId = 1, Items = new List<OrderItem> { new OrderItem { ProductId = 1, Quantity = 1 } } },
                new Order { Id = 2, OrderDate = DateTime.Parse("2/1/2019"), CustomerId = 2, Items = new List<OrderItem> { new OrderItem { ProductId = 2, Quantity = 1 } } },
                new Order { Id = 3, OrderDate = DateTime.Parse("3/1/2019"), CustomerId = 3, Items = new List<OrderItem> { new OrderItem { ProductId = 3, Quantity = 1 } } }
            };
        }

    }
}
