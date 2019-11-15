using GroceryStore;
using GroceryStore.Entity;
using GroceryStore.Utility;
using System;


namespace SandBox
{
    class Program
    {
        /// <summary>
        /// NOTE: before this can work, the app config key "DbFilePath" must be updated to reflect the location of "database.json"
        /// on the local machine
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {            
            // get an order, make changes & save
            var dataBroker = Factory.GetDataBroker();
            var store = new GroceryStoreManager(dataBroker);
            var entity = store.GetOrder(1);
            entity.CustomerId = 3;
            entity.OrderDate = DateTime.Now;
            entity.Items.Clear();
            entity.Items.Add(new OrderItem { ProductId = 1, Quantity = 1 });
            store.Save(entity);

            //make more changes and save again
            entity.CustomerId = 4;
            entity.OrderDate = DateTime.Now;
            entity.Items.Add(new OrderItem { ProductId = 2, Quantity = 2 });
            store.Save(entity);

            Console.ReadLine();
        }
    }
}
