using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Forename { get; set; } = default!;
    public required string Surname { get; set; } = default!;
    public required string Email { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}
