using System;

namespace UserManagement.Web.Models.Users;

public class UserDetailsViewModel
{
    public long Id { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string DisplayDateOfBirth =>
        !DateOfBirth.HasValue || DateOfBirth.Value == DateOnly.MinValue
            ? "Not provided"
            : DateOfBirth.Value.ToString("dd/MM/yyyy");

    public bool IsActive { get; set; }
}
