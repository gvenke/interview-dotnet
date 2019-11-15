using System.Runtime.Serialization;
using GroceryStore.DataBroker;

namespace GroceryStore.Entity
{
    [DataContract]
    public class Product : EntityWithIdBase
    {

        protected override bool HasBeenChanged()
        {
            var checkPoint = (Product)_checkPoint;
            return Price != checkPoint.Price || Description != checkPoint.Description || checkPoint.Id != Id;
        }

        protected override EntityBase CreateNewCheckPoint()
        {
            var checkPoint = (Product)MemberwiseClone();
            return checkPoint;
        }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public double Price { get; set; }

        protected override void SaveNew(IDataBroker dataBroker)
        {
            dataBroker.SaveProduct(this);
        }

        protected override void SaveExisting(IDataBroker dataBroker)
        {
           dataBroker.UpdateProduct(this);
        }
    }
}
