using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Family : BaseModel
{
    public string FamilyName { get; set; }

    public Family (Guid id, string familyName)
    {
        Id = id;
        FamilyName = familyName;
    }
}