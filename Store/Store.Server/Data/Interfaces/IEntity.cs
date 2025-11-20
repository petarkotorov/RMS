namespace Store.API.Data.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedOn { get; set; }
    }
}
