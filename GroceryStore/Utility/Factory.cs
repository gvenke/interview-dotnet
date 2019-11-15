using GroceryStore.DataBroker;
using GroceryStore.Entity;
using System.Configuration;

namespace GroceryStore.Utility
{
    /// <summary>
    /// good for encapsulating logic that determines how objects are populated on creation or 
    /// whether to generate a subclass. It's pretty basic now, but can be easily scaled
    /// </summary>
    public static class Factory
    {
        public const string DbFilePathKey = "DbFilePath";

        public static Product CreateProduct()
        {
            return new Product();
        }

        public static Customer CreateCustomer()
        {
            return new Customer();
        }

        public static Order CreateOrder()
        {
            return new Order();
        }

        /// <summary>
        /// good for encapsulating logic that determines which implemention to generate. 
        /// </summary>
        /// <returns></returns>
        public static IDataBroker GetDataBroker()
        {
            return new JsonDataBroker(ConfigurationManager.AppSettings[DbFilePathKey]);
        }
    }
}
