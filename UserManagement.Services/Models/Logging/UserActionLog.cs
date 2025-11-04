using System;

namespace UserManagement.Services.Models.Logging;    

public class UserActionLog
{
    public int Id { get; set; }         //Log entry
    public int UserId { get; set; }     //User affected
    public string Action { get; set; } = string.Empty; //Action performed
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Details { get; set; } //Additional info
}

