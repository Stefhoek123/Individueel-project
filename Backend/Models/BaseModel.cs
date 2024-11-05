namespace Models;

public abstract class BaseModel : IBaseModel
{
    public Guid Id { get; set; }
}