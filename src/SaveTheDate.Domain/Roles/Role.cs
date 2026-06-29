using SaveTheDate.Domain.Common;

namespace SaveTheDate.Domain.Roles;

public class Role : BaseEntity
{
    public string Description { get; private set; } = string.Empty;

    private Role() { }

    public Role(string description)
    {
        SetDescription(description);
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("Role description cannot be empty.");
        Description = description;
    }
}
