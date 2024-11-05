namespace Models;

public class Family
{
    public int FamilyId { get; set; }
    public string FamilyName { get; set; }

    public User UserId { get; set; }

    public Family (int familyId, string familyName, User userId)
    {
        FamilyId = familyId;
        FamilyName = familyName;
        UserId = userId;
    }
}