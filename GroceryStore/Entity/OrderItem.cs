using System.Runtime.Serialization;

namespace GroceryStore.Entity
{
    [DataContract]
    public class OrderItem
    {

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }

    }
}
