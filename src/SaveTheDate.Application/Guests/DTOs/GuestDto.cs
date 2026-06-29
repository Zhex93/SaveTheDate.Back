namespace SaveTheDate.Application.Guests.DTOs;

public record GuestDto(int Id, string Name, string Surname, string Email, string Phone, int RoleId);
