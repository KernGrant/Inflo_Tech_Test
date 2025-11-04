using System;

namespace UserManagement.Data.Entities;

public class UserActionLog
{
    public int Id { get; set; }         //Log entry Id
    public int UserId { get; set; }     //User affected
    public string Action { get; set; } = string.Empty; //Action performed
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Details { get; set; } //Additional info
}

