using System.Runtime.Serialization;

namespace GroceryStore.Entity
{
    /// <summary>
    /// since all current entities have nullable int Id props, it makes sense to encapsulate
    /// in a separate base class. This allows the flexibility to have additional entity
    /// classes with the Id prop, or not.
    /// </summary>
    /// 
    [DataContract]
    public abstract class EntityWithIdBase : EntityBase
    {
        [DataMember]
        public int? Id { get; set; }

        public override bool IsNew()
        {
            return Id == null;
        }
    }
}
