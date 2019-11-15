using System.Runtime.Serialization;
using GroceryStore.DataBroker;

namespace GroceryStore.Entity
{
    [DataContract]
    public class Customer : EntityWithIdBase
    {

        protected override bool HasBeenChanged()
        {
            var checkPoint = (Customer)_checkPoint;
            return Name != checkPoint.Name || checkPoint.Id != Id;
        }

        protected  override EntityBase CreateNewCheckPoint()
        {
            var checkPoint = (Customer)MemberwiseClone();          
            return checkPoint;
        }

        [DataMember]
        public string Name { get; set; }

        protected override void SaveNew(IDataBroker dataBroker)
        {
            dataBroker.SaveCustomer(this);
        }

        protected override void SaveExisting(IDataBroker dataBroker)
        {
            dataBroker.UpdateCustomer(this);
        }
    }
}
