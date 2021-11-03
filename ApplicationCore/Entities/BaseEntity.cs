namespace RepairMarketPlace.ApplicationCore.Entities
{
    public abstract class BaseEntity
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual int Id { get; protected set; }
    }
}
