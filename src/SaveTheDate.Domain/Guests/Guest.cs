using SaveTheDate.Domain.Common;

namespace SaveTheDate.Domain.Guests;

public class Guest : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Surname { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public int RoleId { get; private set; }

    private Guest() { }

    public Guest(string name, string surname, string email, string phone, int roleId)
    {
        SetName(name);
        SetSurname(surname);
        SetEmail(email);
        SetPhone(phone);
        RoleId = roleId;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Guest name cannot be empty.");
        Name = name;
    }

    public void SetSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname))
            throw new ValidationException("Guest surname cannot be empty.");
        Surname = surname;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("Guest email cannot be empty.");
        Email = email;
    }

    public void SetPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ValidationException("Guest phone cannot be empty.");
        Phone = phone;
    }

    public void SetRole(int roleId)
    {
        RoleId = roleId;
    }

    public void Update(string name, string surname, string email, string phone, int roleId)
    {
        SetName(name);
        SetSurname(surname);
        SetEmail(email);
        SetPhone(phone);
        RoleId = roleId;
    }
}
